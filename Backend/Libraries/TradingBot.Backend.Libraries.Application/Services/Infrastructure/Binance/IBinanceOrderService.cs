using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance
{
	public interface IBinanceOrderService
	{
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
	}
}
