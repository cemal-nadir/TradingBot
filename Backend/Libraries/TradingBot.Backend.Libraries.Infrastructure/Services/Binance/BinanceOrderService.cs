
using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients;
using CNG.Core.Exceptions;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;
using TradingBot.Backend.Libraries.Domain.Enums;
using TradingBot.Backend.Libraries.Domain.Defaults;

namespace TradingBot.Backend.Libraries.Infrastructure.Services.Binance
{
	public class BinanceOrderService : IBinanceOrderService
	{
		private readonly IBinanceRestClient _binanceRestClient;

		public BinanceOrderService(IBinanceRestClient binanceRestClient)
		{
			_binanceRestClient = binanceRestClient;
		}

		public async Task<(decimal,decimal)> CalculateQuantityAndMarketPriceFutures(string symbol, decimal usdtAmount, int leverage = 1,
			CancellationToken cancellationToken = default)
		{
			var response = await _binanceRestClient.UsdFuturesApi.ExchangeData.GetMarkPriceAsync(symbol, cancellationToken);
			if (!response.Success)
				throw new BadRequestException(response.Error?.Message ?? Error.BadRequest.RemoteApiErrorResponse);
			return (Math.Round(usdtAmount * leverage / response.Data.MarkPrice,3),response.Data.MarkPrice);
		}
		public async Task<(decimal, decimal)> CalculateQuantityAndMarketPriceSpot(string symbol, decimal usdtAmount,
			CancellationToken cancellationToken = default)
		{
			var response = await _binanceRestClient.SpotApi.ExchangeData.GetPriceAsync(symbol, cancellationToken);
			if (!response.Success)
				throw new BadRequestException(response.Error?.Message ?? Error.BadRequest.RemoteApiErrorResponse);
			return (Math.Round(usdtAmount / response.Data.Price,3), response.Data.Price);
		}

		public async Task<long> FuturesLong(string symbol, decimal quantity, int leverage = 1, MarginType marginType = MarginType.Isolated,
			CancellationToken cancellationToken = default) =>
			await PlaceMarketOrderAsync(symbol, OrderType.Futures, Side.Long, quantity, leverage: leverage, marginType: marginType, cancellationToken: cancellationToken);


		public async Task<long> FuturesShort(string symbol, decimal quantity, int leverage = 1, MarginType marginType = MarginType.Isolated,
			CancellationToken cancellationToken = default) =>
			await PlaceMarketOrderAsync(symbol, OrderType.Futures, Side.Short, quantity, leverage: leverage, marginType: marginType, cancellationToken: cancellationToken);
		public async Task<long> SpotLong(string symbol, decimal quantity,
			CancellationToken cancellationToken = default) =>
			await PlaceMarketOrderAsync(symbol, OrderType.Spot, Side.Long, quantity, cancellationToken: cancellationToken);

		public async Task<long> SpotShort(string symbol, decimal quantity,
			CancellationToken cancellationToken = default) =>
			await PlaceMarketOrderAsync(symbol, OrderType.Spot, Side.Short, quantity, cancellationToken: cancellationToken);
		public async Task CloseFuturesOrdersAndPositionsAsync(string symbol,
			CancellationToken cancellationToken = default)
		{
			var cancelOrders = _binanceRestClient.UsdFuturesApi.Trading.CancelAllOrdersAsync(symbol, null, cancellationToken);
			var positions = _binanceRestClient.UsdFuturesApi.Account.GetPositionInformationAsync(symbol, null,
				cancellationToken);
			await Task.WhenAll(cancelOrders, positions);
			var currentPosition = positions.Result.Data.FirstOrDefault();
			if (currentPosition is null) return;

			await PlaceMarketOrderAsync(currentPosition.Symbol, OrderType.Futures,
				currentPosition.PositionSide is PositionSide.Long ? Side.Short : Side.Long,
				currentPosition.Quantity, closePosition: true, cancellationToken: cancellationToken, leverage: null, marginType: null);

		}
	
		public async Task CloseSpotOrdersAsync(string symbol,
			CancellationToken cancellationToken = default) =>
			await _binanceRestClient.SpotApi.Trading.CancelAllOrdersAsync(symbol, null, cancellationToken);

		private async Task<long> PlaceMarketOrderAsync(string symbol, OrderType orderType, Side side, decimal quantity, bool? closePosition = false, int? leverage = 1, MarginType? marginType = MarginType.Isolated, CancellationToken cancellationToken = default)
		{
			long orderId;
			switch (orderType)
			{
				case OrderType.Futures:
				default:
					{
						if (leverage != null)
							await _binanceRestClient.UsdFuturesApi.Account.ChangeInitialLeverageAsync(symbol, leverage.Value,
								null, cancellationToken);
						if (marginType != null)
							await _binanceRestClient.UsdFuturesApi.Account.ChangeMarginTypeAsync(symbol, marginType is MarginType.Cross ? FuturesMarginType.Cross : FuturesMarginType.Isolated, null, cancellationToken);

						var order = await _binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(
						symbol,
						side is Side.Long ? OrderSide.Buy : OrderSide.Sell,
						FuturesOrderType.Market,
						quantity: Math.Round(quantity, 3),
						reduceOnly: closePosition != null && closePosition.Value,
						timeInForce: TimeInForce.GoodTillCanceled, ct: cancellationToken);

						if (!order.Success)
							throw new BadRequestException(order.Error?.Message ?? Error.BadRequest.RemoteApiErrorResponse);
						orderId = order.Data.Id;
						break;
					}
				case OrderType.Spot:
					{
						var order = await _binanceRestClient.SpotApi.Trading.PlaceOrderAsync(
							symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell,
							SpotOrderType.Market,
							quantity: Math.Round(quantity, 3),
							timeInForce: TimeInForce.GoodTillCanceled, ct: cancellationToken);
						if (!order.Success)
							throw new BadRequestException(order.Error?.Message ?? Error.BadRequest.RemoteApiErrorResponse);
						orderId = order.Data.Id;
						break;
					}

			}
			return orderId;
		}

		public async Task StopLossAndTakeProfitAsync(string symbol, OrderType orderType, Side side, decimal quantity,
			decimal? stopPrice=null,decimal? takeProfitPrice=null, CancellationToken cancellationToken=default)
		{
			if(stopPrice is null&&takeProfitPrice is null)return;

			switch (orderType)
			{
				case OrderType.Futures:
					default:
					if (stopPrice != null && takeProfitPrice != null)
					{
						var stop= _binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, FuturesOrderType.StopMarket,
							quantity: Math.Round(quantity, 3), closePosition: true, stopPrice: stopPrice, ct: cancellationToken);
						var takeProfit= _binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, FuturesOrderType.TakeProfitMarket,
							quantity: Math.Round(quantity, 3), closePosition: true, stopPrice: takeProfitPrice, ct: cancellationToken);
						await Task.WhenAll(stop, takeProfit);
					}
					else if (stopPrice != null)
						await _binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, FuturesOrderType.StopMarket,
							quantity: Math.Round(quantity, 3), closePosition: true, stopPrice: stopPrice, ct: cancellationToken);
					else if(takeProfitPrice!=null)
						await _binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, FuturesOrderType.TakeProfitMarket,
							quantity: Math.Round(quantity, 3), closePosition: true, stopPrice: takeProfitPrice, ct: cancellationToken);

				
					break;
				case OrderType.Spot:
					if (stopPrice != null && takeProfitPrice != null)
					{
						var stop = _binanceRestClient.SpotApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, SpotOrderType.StopLoss,
							quantity: Math.Round(quantity, 3), stopPrice: stopPrice, ct: cancellationToken);
						var takeProfit = _binanceRestClient.SpotApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, SpotOrderType.TakeProfit,
							quantity: Math.Round(quantity, 3), stopPrice: takeProfitPrice, ct: cancellationToken);
						await Task.WhenAll(stop, takeProfit);
					}
					else if (stopPrice != null)
						await _binanceRestClient.SpotApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, SpotOrderType.StopLoss,
							quantity: Math.Round(quantity, 3), stopPrice: stopPrice, ct: cancellationToken);
					else if (takeProfitPrice != null)
						await _binanceRestClient.SpotApi.Trading.PlaceOrderAsync(symbol,
							side is Side.Long ? OrderSide.Buy : OrderSide.Sell, SpotOrderType.TakeProfit,
							quantity: Math.Round(quantity, 3), stopPrice: takeProfitPrice, ct: cancellationToken);
					break;
			}
		}

		public async Task TrailingStopLossFuture(string symbol, Side side, decimal quantity, decimal callbackRate,
			decimal activationPrice, CancellationToken cancellationToken = default)
		{
			await _binanceRestClient.UsdFuturesApi.Trading.PlaceOrderAsync(symbol,
				side is Side.Long ? OrderSide.Buy : OrderSide.Sell, FuturesOrderType.TrailingStopMarket, Math.Round(quantity, 3),
				activationPrice: activationPrice, callbackRate: callbackRate, ct: cancellationToken);
		}
	}
}
