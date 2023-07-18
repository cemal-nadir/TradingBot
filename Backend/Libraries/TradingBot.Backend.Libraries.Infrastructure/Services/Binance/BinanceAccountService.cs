using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Spot;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;
using TradingBot.Backend.Libraries.Infrastructure.Extensions;

namespace TradingBot.Backend.Libraries.Infrastructure.Services.Binance
{
	public class BinanceAccountService : IBinanceAccountService
	{
		private readonly IBinanceRestClient _binanceRestClient;

		public BinanceAccountService(IBinanceRestClient binanceRestClient)
		{
			_binanceRestClient = binanceRestClient;
		}

		public async Task<BinanceAccountInfo> GetAccountInfoSpot(
			CancellationToken cancellationToken = default) =>
			(await _binanceRestClient.SpotApi.Account.GetAccountInfoAsync(null, cancellationToken)).CheckError();

		public async Task<List<BinanceUserBalance>> GetAccountBalanceSpot(CancellationToken cancellationToken = default)
		{
			return (await _binanceRestClient.SpotApi.Account.GetBalancesAsync(null,null,null, cancellationToken)).CheckError().ToList();
		}
		public async Task<BinanceFuturesAccountInfo> GetAccountInfoFutures(
			CancellationToken cancellationToken = default) =>
			(await _binanceRestClient.UsdFuturesApi.Account.GetAccountInfoAsync(null, cancellationToken)).CheckError();
		public async Task<List<BinanceUsdFuturesAccountBalance>> GetAccountBalanceFutures(
			CancellationToken cancellationToken = default) =>
			(await _binanceRestClient.UsdFuturesApi.Account.GetBalancesAsync(null, cancellationToken)).CheckError().ToList();
	}
}
