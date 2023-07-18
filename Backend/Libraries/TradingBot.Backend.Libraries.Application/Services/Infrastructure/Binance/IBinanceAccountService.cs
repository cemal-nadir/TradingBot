using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Spot;

namespace TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance
{
	public interface IBinanceAccountService
	{
		Task<BinanceAccountInfo> GetAccountInfoSpot(
			CancellationToken cancellationToken = default);

		Task<List<BinanceUserBalance>> GetAccountBalanceSpot(CancellationToken cancellationToken = default);

		Task<BinanceFuturesAccountInfo> GetAccountInfoFutures(
			CancellationToken cancellationToken = default);

		Task<List<BinanceUsdFuturesAccountBalance>> GetAccountBalanceFutures(
			CancellationToken cancellationToken = default);
	}
}
