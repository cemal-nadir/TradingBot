using CNG.Core.Exceptions;
using DotNetCore.CAP;
using TradingBot.Backend.Libraries.Application.Dtos.Cap;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;
using TradingBot.Backend.Libraries.Domain.Defaults;
using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Services.Binance.API.Caps;

public class PullSubscribeService : ICapSubscribe
{
	private readonly IBinanceMainClient _binanceMainClient;
	public PullSubscribeService(IBinanceMainClient binanceMainClient)
	{
		_binanceMainClient = binanceMainClient;
	}

	[CapSubscribe(Cap.BinanceHook)]
	public async Task<HookResponseDto?> BinanceHook(HookDto model, CancellationToken cancellationToken = default)
	{
		if (model.IndicatorHook?.Order?.Symbol is null) return null;
		if (!Enum.TryParse(model.IndicatorHook.Order?.OrderType, out OrderType orderType) ||
			!Enum.TryParse(model.IndicatorHook.Order?.Side, out Side side)) return null;
		_binanceMainClient.SetClient(model.ApiKey ?? "", model.SecretKey ?? "");


		HookResponseTradingHistory? responseDto = default;
		decimal currentBalance;
		switch (orderType)
		{
			case OrderType.Spot:
				{
					await _binanceMainClient.BinanceOrderService.CloseSpotOrdersAsync(
						model.IndicatorHook.Order.Symbol,
						cancellationToken);
					var balanceAndQuantity =await GetCurrentBalanceAndQuantityAndMarkPrice(
						model.IndicatorHook?.Order?.Symbol ?? "",
						model.CurrentAdjustedBalance,
						OrderType.Spot, cancellationToken: cancellationToken);
					
					currentBalance = balanceAndQuantity.Item1.TotalBalance;
					if (balanceAndQuantity.Item1.TotalBalance < model.MinimumBalance ||
						balanceAndQuantity.Item1.AvailableFuturesBalance <
						model.CurrentAdjustedBalance || balanceAndQuantity.Item2 is 0 || model.CurrentAdjustedBalance < model.MinimumBalance)
						break;

					_ = side is Side.Short
						? await _binanceMainClient.BinanceOrderService.SpotShort(
							model.IndicatorHook?.Order?.Symbol ?? "", model.TradingHistoryQuantity ?? 0, cancellationToken)
						: await _binanceMainClient.BinanceOrderService.SpotLong(
							model.IndicatorHook?.Order?.Symbol ?? "", balanceAndQuantity.Item2, cancellationToken);


					await SetSlAndTpAndTsl(model.IndicatorHook?.Order?.Symbol!, side,
						OrderType.Spot, balanceAndQuantity.Item3, balanceAndQuantity.Item2,
						model.IndicatorHook?.TakeProfitPercentage, model.IndicatorHook?.StopLossPercentage,
						cancellationToken: cancellationToken);

					responseDto = new()
					{
						Side = model.IndicatorHook!.Order!.Side,
						Symbol = model.IndicatorHook.Order.Symbol,
						OrderType = model.IndicatorHook.Order.OrderType,
						EntryPrice = balanceAndQuantity.Item3,
						Quantity = balanceAndQuantity.Item2,
					};
					break;
				}
			case OrderType.Futures:
			default:
				{
					await _binanceMainClient.BinanceOrderService.CloseFuturesOrdersAndPositionsAsync(model.IndicatorHook?.Order?
						.Symbol ?? "", cancellationToken);
					var assetInfo =
						await _binanceMainClient.BinanceOrderService.GetExchangeInfoFutures(model.IndicatorHook?.Order?
							.Symbol??"", cancellationToken);
					var balanceAndQuantity = await GetCurrentBalanceAndQuantityAndMarkPrice(
						model.IndicatorHook?.Order?.Symbol ?? "",
						model.CurrentAdjustedBalance,
						OrderType.Futures,
						model.IndicatorHook?.Order?.Leverage ?? 1,
						cancellationToken);

					currentBalance = balanceAndQuantity.Item1.TotalBalance;
					if (model.IndicatorHook?.Order?.ClosePosition != null &&
						model.IndicatorHook.Order.ClosePosition.Value) break;
					if (balanceAndQuantity.Item1.TotalBalance < model.MinimumBalance ||
					balanceAndQuantity.Item1.AvailableFuturesBalance <
					model.CurrentAdjustedBalance || balanceAndQuantity.Item2 is 0 || model.CurrentAdjustedBalance < model.MinimumBalance)
						break;

					if (!Enum.TryParse(model.IndicatorHook?.Order?.MarginType, out MarginType marginType)) return null;

					_ = side is Side.Long
						? await _binanceMainClient.BinanceOrderService.FuturesLong(model.IndicatorHook?.Order?.Symbol!,
							 Math.Round(balanceAndQuantity.Item2,assetInfo?.QuantityPrecision ?? 3) , model.IndicatorHook?.Order?.Leverage ?? 1,
							marginType, cancellationToken)
						: await _binanceMainClient.BinanceOrderService.FuturesShort(
							model.IndicatorHook?.Order?.Symbol ?? "", Math.Round(balanceAndQuantity.Item2, assetInfo?.QuantityPrecision ?? 3),
							model.IndicatorHook?.Order?.Leverage ?? 1,
							marginType, cancellationToken);

					await SetSlAndTpAndTsl(model.IndicatorHook!.Order!.Symbol, side,
						OrderType.Futures, balanceAndQuantity.Item3, balanceAndQuantity.Item2,
						model.IndicatorHook?.TakeProfitPercentage, model.IndicatorHook?.StopLossPercentage,
						model.IndicatorHook?.TrailingStopLoss?.CallBackPercentage,
						model.IndicatorHook?.TrailingStopLoss?.ActivationPercentage,assetInfo?.QuantityPrecision,assetInfo?.PricePrecision,cancellationToken);

					responseDto = new()
					{
						Side = model.IndicatorHook!.Order.Side,
						Symbol = model.IndicatorHook.Order.Symbol,
						OrderType = model.IndicatorHook.Order.OrderType,
						EntryPrice = balanceAndQuantity.Item3,
						Quantity = balanceAndQuantity.Item2,
					};
					break;
				}
		}

		return new HookResponseDto()
		{
			CloseTradingHistoryId = model.TradingHistoryId,
			TradingHistory = responseDto,
			CurrentBalance = currentBalance,
			TradingAccountId = model.TradingAccountId
		};
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
	/// <param name="pricePrecision"></param>
	/// <param name="cancellationToken"></param>
	/// <param name="quantityPrecision"></param>
	/// <returns></returns>
	private async Task SetSlAndTpAndTsl(string symbol, Side side, OrderType orderType, decimal markPrice, decimal quantity, decimal? takeProfitPercentage = null, decimal? stopLossPercentage = null, decimal? trailingStopLossCallBackPercentage = null, decimal? trailingStopLossActivationPercentage = null,int? quantityPrecision=3,int?pricePrecision=2, CancellationToken cancellationToken = default)
	{
		if (takeProfitPercentage is null && stopLossPercentage is null && trailingStopLossCallBackPercentage is null)
			return;

		if (trailingStopLossCallBackPercentage != null)
			await _binanceMainClient.BinanceOrderService.TrailingStopLossFuture(
				symbol,
				side is Side.Long ? Side.Short : Side.Long,
				Math.Round(quantity,quantityPrecision!.Value) , trailingStopLossCallBackPercentage.Value,
				trailingStopLossActivationPercentage is null or 0 or < 0 ? 0 : side is Side.Long ? Math.Round(markPrice + markPrice * trailingStopLossActivationPercentage!.Value / 100, pricePrecision!.Value) :Math.Round(markPrice - markPrice * trailingStopLossActivationPercentage!.Value / 100, pricePrecision!.Value) , cancellationToken);



		if (takeProfitPercentage != null || stopLossPercentage != null)
			await _binanceMainClient.BinanceOrderService.StopLossAndTakeProfitAsync(
				symbol,
				orderType,
				side is Side.Long ? Side.Short : Side.Long, Math.Round(quantity,quantityPrecision!.Value),
				stopPrice: stopLossPercentage is null ? null : side is Side.Long
					? Math.Round((markPrice - (markPrice *
						stopLossPercentage.Value / 100)),pricePrecision!.Value) 
					: Math.Round((markPrice + (markPrice *
						stopLossPercentage.Value / 100)),pricePrecision!.Value),
				takeProfitPrice: takeProfitPercentage is null ? null : side is Side.Long
					? Math.Round((markPrice + (markPrice *
						takeProfitPercentage.Value / 100)),pricePrecision!.Value)
					: Math.Round((markPrice - (markPrice *
						takeProfitPercentage.Value / 100)),pricePrecision!.Value), cancellationToken: cancellationToken);

	}
	private async Task<(Balance, decimal, decimal)> GetCurrentBalanceAndQuantityAndMarkPrice(string symbol, decimal? currentAdjustedBalance, OrderType orderType, int leverage = 1, CancellationToken cancellationToken = default)
	{
		var futuresBalances = await
			_binanceMainClient.BinanceAccountService.GetAccountBalanceFutures(cancellationToken);

		var spotBalances = await
			_binanceMainClient.BinanceAccountService.GetAccountBalanceSpot(cancellationToken);

		var balance = new Balance
		{
			AvailableFuturesBalance = futuresBalances
				                          .FirstOrDefault(x =>
					                          x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
				                          ?.AvailableBalance ??
			                          throw new NotFoundException("Balance not found"),
			AvailableSpotBalance = spotBalances
				                       .FirstOrDefault(x =>
					                       x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
				                       ?.Available ??
			                       throw new NotFoundException("Balance not found"),
			FuturesBalance = futuresBalances
				                 .FirstOrDefault(x =>
					                 x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)
				                 ?.WalletBalance ??
			                 throw new NotFoundException("Balance not found"),
			SpotBalance = spotBalances
				              .FirstOrDefault(x =>
					              x.Asset == Libraries.Domain.Defaults.TradingPlatform.DefaultAsset)?.Total ??
			              throw new NotFoundException("Balance not found")
		};

			var quantityResponse = orderType is OrderType.Futures
				? await
					_binanceMainClient.BinanceOrderService.CalculateQuantityAndMarketPriceFutures(symbol,
						currentAdjustedBalance is null or 0 ? balance.AvailableFuturesBalance:currentAdjustedBalance.Value, leverage,
						cancellationToken)
				: await _binanceMainClient.BinanceOrderService.CalculateQuantityAndMarketPriceSpot(symbol,
					currentAdjustedBalance is null or 0 ? balance.AvailableSpotBalance: currentAdjustedBalance.Value,
					cancellationToken);

		return (balance, quantityResponse.Item1, quantityResponse.Item2);

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