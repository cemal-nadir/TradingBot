using CNG.Core.Exceptions;
using DotNetCore.CAP;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;
using TradingBot.Backend.Libraries.Domain.Defaults;
using TradingBot.Backend.Libraries.Domain.Enums;
using TradingBot.Backend.Services.Binance.API.Models;

namespace TradingBot.Backend.Services.Binance.API.Caps;

public class PullSubscribeService : ICapSubscribe
{
	private readonly IBinanceMainClient _binanceMainClient;
	private readonly ICapPublisher _capPublisher;
	public PullSubscribeService(IBinanceMainClient binanceMainClient, ICapPublisher capPublisher)
	{
		_binanceMainClient = binanceMainClient;
		_capPublisher = capPublisher;
	}

	[CapSubscribe(Cap.BinanceHook)]
	public async Task HookAsync(HookModel model, CancellationToken cancellationToken = default)
	{
		if (model.Account is null || model.IndicatorHook?.Order?.Symbol is null|| model.IndicatorHook?.Order?.OrderType is null||model.IndicatorHook?.Order?.Side is null) return;

		_binanceMainClient.SetClient(model.Account.ApiKey ?? "", model.Account.SecretKey ?? "");
		TradingHistoryDto? responseDto=default;
		decimal currentBalance;
		switch (model.IndicatorHook.Order.OrderType)
		{
			case OrderType.Spot:
			{
				var closeOrders = _binanceMainClient.BinanceOrderService.CloseSpotOrdersAsync(
					model.IndicatorHook.Order.Symbol,
					cancellationToken);
				var balanceAndQuantity = GetCurrentBalanceAndQuantityAndMarkPrice(
					model.IndicatorHook?.Order?.Symbol ?? "",
					model.Account.BalanceSettings?.CurrentAdjustedBalance,
					OrderType.Spot, cancellationToken: cancellationToken);
				await Task.WhenAll(closeOrders, balanceAndQuantity);
				currentBalance = balanceAndQuantity.Result.Item1.TotalBalance;
				if (balanceAndQuantity.Result.Item1.TotalBalance < model.Account?.BalanceSettings?.MinimumBalance ||
				    balanceAndQuantity.Result.Item1.AvailableFuturesBalance <
				    model.Account?.BalanceSettings?.CurrentAdjustedBalance || balanceAndQuantity.Result.Item2 is 0)
					break;

				_ = model.IndicatorHook?.Order?.Side is Side.Short
					? await _binanceMainClient.BinanceOrderService.SpotShort(
						model.IndicatorHook?.Order?.Symbol ?? "", model.TradingHistory?.Quantity ?? 0, cancellationToken)
					: await _binanceMainClient.BinanceOrderService.SpotLong(
						model.IndicatorHook?.Order?.Symbol ?? "", balanceAndQuantity.Result.Item2, cancellationToken);


				await SetSlAndTpAndTsl(model.IndicatorHook!.Order!.Symbol, model.IndicatorHook.Order.Side!.Value,
					model.IndicatorHook.Order.OrderType, balanceAndQuantity.Result.Item3, balanceAndQuantity.Result.Item2,
					model.IndicatorHook?.TakeProfitPercentage, model.IndicatorHook?.StopLossPercentage,
					cancellationToken: cancellationToken);
				responseDto = new()
				{
					Side = model.IndicatorHook!.Order.Side.Value,
					Symbol = model.IndicatorHook.Order.Symbol,
					OrderType = model.IndicatorHook.Order.OrderType,
					EntryPrice = balanceAndQuantity.Result.Item3,
					TradingAccountId = model.Account?.Id,
					IsClosed = false,
					Quantity = balanceAndQuantity.Result.Item2,
				};
				break;
			}
			case OrderType.Futures:
			default:
			{
				await _binanceMainClient.BinanceOrderService.CloseFuturesOrdersAndPositionsAsync(model.IndicatorHook?.Order?
					.Symbol ?? "", cancellationToken);

				

				var balanceAndQuantity = await GetCurrentBalanceAndQuantityAndMarkPrice(
					model.IndicatorHook?.Order?.Symbol ?? "",
					model.Account.BalanceSettings?.CurrentAdjustedBalance,
					OrderType.Futures,
					model.IndicatorHook?.Order?.Leverage ?? 1,
					cancellationToken);
				currentBalance = balanceAndQuantity.Item1.TotalBalance;
				if (model.IndicatorHook?.Order?.ClosePosition != null &&
				    model.IndicatorHook.Order.ClosePosition.Value) break;
					if (balanceAndQuantity.Item1.TotalBalance < model.Account?.BalanceSettings?.MinimumBalance ||
				    balanceAndQuantity.Item1.AvailableFuturesBalance <
				    model.Account?.BalanceSettings?.CurrentAdjustedBalance || balanceAndQuantity.Item2 is 0)
					break;


				_ = model.IndicatorHook?.Order?.Side is Side.Long
					? await _binanceMainClient.BinanceOrderService.FuturesLong(model.IndicatorHook.Order.Symbol,
						balanceAndQuantity.Item2, model.IndicatorHook?.Order?.Leverage ?? 1,
						model.IndicatorHook?.Order?.MarginType ?? MarginType.Isolated, cancellationToken)
					: await _binanceMainClient.BinanceOrderService.FuturesShort(
						model.IndicatorHook?.Order?.Symbol ?? "", balanceAndQuantity.Item2,
						model.IndicatorHook?.Order?.Leverage ?? 1,
						model.IndicatorHook?.Order?.MarginType ?? MarginType.Isolated, cancellationToken);

				await SetSlAndTpAndTsl(model.IndicatorHook!.Order!.Symbol, model.IndicatorHook.Order.Side!.Value,
					model.IndicatorHook.Order.OrderType, balanceAndQuantity.Item3, balanceAndQuantity.Item2,
					model.IndicatorHook?.TakeProfitPercentage, model.IndicatorHook?.StopLossPercentage,
					model.IndicatorHook?.TrailingStopLoss?.CallBackPercentage,
					model.IndicatorHook?.TrailingStopLoss?.ActivationPercentage, cancellationToken);

				responseDto = new()
				{
					Side = model.IndicatorHook!.Order.Side.Value,
					Symbol = model.IndicatorHook.Order.Symbol,
					OrderType = model.IndicatorHook.Order.OrderType,
					EntryPrice = balanceAndQuantity.Item3,
					TradingAccountId = model.Account?.Id,
					IsClosed = false,
					Quantity = balanceAndQuantity.Item2
				};
					break;
			}
		}

		await _capPublisher.PublishAsync(Cap.HookResponse, new HookResponseModel()
		{
			CloseTradingHistoryId = model.TradingHistory?.Id,
			TradingHistory = responseDto,
			CurrentBalance = currentBalance
		}, cancellationToken: cancellationToken);
	}

	/// <summary>
	/// Set Stop Loss, Take Profit, Trailing Stop Loss 
	/// </summary>
	/// <param name="symbol">BTCUSDT,ETHUSDT</param>
	/// <param name="side">Long or Short</param>
	/// <param name="orderType"> Futures or Spot</param>
	/// <param name="markPrice">Mark Price</param>
	/// <param name="quantity">Quantity of symbols to be processed</param>
	/// <param name="takeProfitPercentage">Take Profit Percentage</param>
	/// <param name="stopLossPercentage">Stop Loss Percentage</param>
	/// <param name="trailingStopLossCallBackPercentage">Trailing Stop Loss Percentage</param>
	/// <param name="trailingStopLossActivationPercentage">Trailing Stop Loss Activation Percentage</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	private async Task SetSlAndTpAndTsl(string symbol,Side side, OrderType orderType,decimal markPrice,decimal quantity, decimal? takeProfitPercentage=null,decimal?stopLossPercentage=null,decimal?trailingStopLossCallBackPercentage=null,decimal?trailingStopLossActivationPercentage=null, CancellationToken cancellationToken = default)
	{
		if (takeProfitPercentage is null && stopLossPercentage is null && trailingStopLossCallBackPercentage is null)
			return;

		if (trailingStopLossCallBackPercentage != null && (takeProfitPercentage != null || stopLossPercentage != null))
		{
			var stopLossTakeProfit = _binanceMainClient.BinanceOrderService.StopLossAndTakeProfitAsync(
				symbol,
				orderType,
				side is Side.Long ? Side.Short:Side.Long, quantity,
				stopPrice: stopLossPercentage is null ? null : side is Side.Long
					? (markPrice - (markPrice *
						stopLossPercentage / 100))
					: (markPrice + (markPrice *
						stopLossPercentage / 100)),
				takeProfitPrice: takeProfitPercentage is null ? null : side is Side.Long
					? (markPrice + (markPrice *
						takeProfitPercentage / 100))
					: (markPrice - (markPrice *
						takeProfitPercentage / 100)), cancellationToken: cancellationToken);
			var trailingStopLoss = _binanceMainClient.BinanceOrderService.TrailingStopLossFuture(
				symbol,
				side is Side.Long ? Side.Short : Side.Long,
				quantity, trailingStopLossCallBackPercentage.Value,
				trailingStopLossActivationPercentage??0, cancellationToken);
			await Task.WhenAll(stopLossTakeProfit, trailingStopLoss);
		}
		else
		{
			await _binanceMainClient.BinanceOrderService.StopLossAndTakeProfitAsync(
				symbol,
				orderType,
				side is Side.Long ? Side.Short : Side.Long, quantity,
				stopPrice: stopLossPercentage is null ? null : side is Side.Long
					? (markPrice - (markPrice *
						stopLossPercentage / 100))
					: (markPrice + (markPrice *
						stopLossPercentage / 100)),
				takeProfitPrice: takeProfitPercentage is null ? null : side is Side.Long
					? (markPrice + (markPrice *
						takeProfitPercentage / 100))
					: (markPrice - (markPrice *
						takeProfitPercentage / 100)), cancellationToken: cancellationToken);
		}
	}
	private async Task<(Balance, decimal,decimal)> GetCurrentBalanceAndQuantityAndMarkPrice(string symbol, decimal? currentAdjustedBalance,OrderType orderType, int leverage = 1, CancellationToken cancellationToken = default)
	{
		Balance balance;

		decimal quantity;
		decimal markPrice;
		if (currentAdjustedBalance is null or 0)
		{
			var futuresBalances =
				_binanceMainClient.BinanceAccountService.GetAccountBalanceFutures(cancellationToken);

			var spotBalances =
				_binanceMainClient.BinanceAccountService.GetAccountBalanceSpot(cancellationToken);
				             
			await Task.WhenAll(futuresBalances, spotBalances);
			balance = new()
			{
				AvailableFuturesBalance = futuresBalances.Result
					                          .FirstOrDefault(x =>
						                          x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
					                          ?.AvailableBalance ??
				                          throw new NotFoundException("Balance not found"),
				AvailableSpotBalance = spotBalances.Result
					                       .FirstOrDefault(x =>
						                       x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
					                       ?.Available ??
				                       throw new NotFoundException("Balance not found"),
				FuturesBalance = futuresBalances.Result
					                 .FirstOrDefault(x =>
						                 x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
					                 ?.WalletBalance ??
				                 throw new NotFoundException("Balance not found"),
				SpotBalance = spotBalances.Result
					              .FirstOrDefault(x =>
						              x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)?.Total ??
				              throw new NotFoundException("Balance not found")
			};
			

			var quantityResponse =orderType is OrderType.Futures ? await
				_binanceMainClient.BinanceOrderService.CalculateQuantityAndMarketPriceFutures(symbol, balance.AvailableFuturesBalance, leverage,
					cancellationToken) :await _binanceMainClient.BinanceOrderService.CalculateQuantityAndMarketPriceSpot(symbol, balance.AvailableSpotBalance,
				cancellationToken);
				
			quantity = quantityResponse.Item1;
			markPrice= quantityResponse.Item2;
		}
		else
		{
			var futuresBalances =
				_binanceMainClient.BinanceAccountService.GetAccountBalanceFutures(cancellationToken);

			var spotBalances =
				_binanceMainClient.BinanceAccountService.GetAccountBalanceSpot(cancellationToken);
			var quantityResponse = orderType is OrderType.Futures ? _binanceMainClient.BinanceOrderService.CalculateQuantityAndMarketPriceFutures(symbol,
				currentAdjustedBalance.Value, leverage,
				cancellationToken): _binanceMainClient.BinanceOrderService.CalculateQuantityAndMarketPriceSpot(symbol,
				currentAdjustedBalance.Value,
				cancellationToken);
			await Task.WhenAll(futuresBalances, spotBalances,quantityResponse);
			balance = new()
			{
				AvailableFuturesBalance = futuresBalances.Result
					                          .FirstOrDefault(x =>
						                          x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
					                          ?.AvailableBalance ??
				                          throw new NotFoundException("Balance not found"),
				AvailableSpotBalance = spotBalances.Result
					                       .FirstOrDefault(x =>
						                       x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
					                       ?.Available ??
				                       throw new NotFoundException("Balance not found"),
				FuturesBalance = futuresBalances.Result
					                 .FirstOrDefault(x =>
						                 x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
					                 ?.WalletBalance ??
				                 throw new NotFoundException("Balance not found"),
				SpotBalance = spotBalances.Result
					              .FirstOrDefault(x =>
						              x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)?.Total ??
				              throw new NotFoundException("Balance not found")
			};





			quantity = quantityResponse.Result.Item1;
			markPrice= quantityResponse.Result.Item2;
		}

		return (balance,quantity,markPrice);

	}
	private class Balance
	{
		public decimal SpotBalance { get; set; }
		public decimal FuturesBalance { get; set; }
		public decimal AvailableSpotBalance { get; set; }
		public decimal AvailableFuturesBalance { get; set; }
		public decimal TotalBalance => SpotBalance + FuturesBalance;
	}
}