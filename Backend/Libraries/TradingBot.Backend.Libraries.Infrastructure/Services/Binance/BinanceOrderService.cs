
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

		public async Task<long> FuturesLong(string symbol, decimal quantity, int leverage = 1, MarginType marginType=MarginType.Isolated,
			CancellationToken cancellationToken = default) =>
			await PlaceOrderAsync(symbol, OrderType.Futures, Side.Long, quantity, leverage: leverage,marginType:marginType, cancellationToken: cancellationToken);


		public async Task<long> FuturesShort(string symbol, decimal quantity, int leverage = 1,MarginType marginType=MarginType.Isolated,
			CancellationToken cancellationToken = default) =>
			await PlaceOrderAsync(symbol, OrderType.Futures, Side.Short, quantity, leverage: leverage,marginType:marginType, cancellationToken: cancellationToken);
		public async Task<long> SpotLong(string symbol, decimal quantity,
			CancellationToken cancellationToken = default) =>
			await PlaceOrderAsync(symbol, OrderType.Spot, Side.Long, quantity, cancellationToken: cancellationToken);

		public async Task<long> SpotShort(string symbol, decimal quantity,
			CancellationToken cancellationToken = default) =>
			await PlaceOrderAsync(symbol, OrderType.Spot, Side.Short, quantity, cancellationToken: cancellationToken);

		public async Task CloseFuturesOrdersAndPositionsAsync(string symbol,
			CancellationToken cancellationToken = default)
		{

			var cancelOrders = _binanceRestClient.UsdFuturesApi.Trading.CancelAllOrdersAsync(symbol, null, cancellationToken);
			var positions = _binanceRestClient.UsdFuturesApi.Account.GetPositionInformationAsync(symbol, null,
				cancellationToken);
			await Task.WhenAll(cancelOrders, positions);
			var currentPosition = positions.Result.Data.FirstOrDefault();
			if (currentPosition is null) return;

			await PlaceOrderAsync(currentPosition.Symbol, OrderType.Futures,
				currentPosition.PositionSide is PositionSide.Long ? Side.Short : Side.Long,
				currentPosition.Quantity, closePosition:true, cancellationToken:cancellationToken,leverage:null,marginType:null);

		}
		public async Task CloseSpotOrdersAsync(string symbol,
			CancellationToken cancellationToken = default) =>
			await _binanceRestClient.SpotApi.Trading.CancelAllOrdersAsync(symbol, null, cancellationToken);

		private async Task<long> PlaceOrderAsync(string symbol, OrderType orderType, Side side, decimal quantity, bool? closePosition = false, int? leverage=1,  MarginType? marginType=MarginType.Isolated , CancellationToken cancellationToken = default)
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
						reduceOnly: closePosition!=null&&closePosition.Value,
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
	}
}
