using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance
{
	public interface IBinanceOrderService
	{
		Task<(decimal, decimal)> CalculateQuantityAndMarketPriceFutures(string symbol, decimal usdtAmount, int leverage = 1,
			CancellationToken cancellationToken = default);

		Task<(decimal, decimal)> CalculateQuantityAndMarketPriceSpot(string symbol, decimal usdtAmount,
			CancellationToken cancellationToken = default);

		Task StopLossAndTakeProfitAsync(string symbol, OrderType orderType, Side side,
			decimal quantity,
			decimal? stopPrice = null, decimal? takeProfitPrice = null, CancellationToken cancellationToken = default);
		Task<long> FuturesLong(string symbol, decimal quantity, int leverage = 1,
			MarginType marginType = MarginType.Isolated,
			CancellationToken cancellationToken = default);

		Task<long> FuturesShort(string symbol, decimal quantity, int leverage = 1,
			MarginType marginType = MarginType.Isolated,
			CancellationToken cancellationToken = default);

		Task<long> SpotLong(string symbol, decimal quantity,
			CancellationToken cancellationToken = default);

		Task<long> SpotShort(string symbol, decimal quantity,
			CancellationToken cancellationToken = default);

		Task CloseFuturesOrdersAndPositionsAsync(string symbol,
			CancellationToken cancellationToken = default);

		Task CloseSpotOrdersAsync(string symbol,
			CancellationToken cancellationToken = default);

		Task TrailingStopLossFuture(string symbol, Side side, decimal quantity, decimal callbackRate,
			decimal activationPrice, CancellationToken cancellationToken = default);
	}
}
