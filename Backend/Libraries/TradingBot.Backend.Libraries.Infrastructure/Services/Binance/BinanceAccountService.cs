using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Futures;
using Binance.Net.Objects.Models.Spot;
using CNG.Core.Exceptions;
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
			(await _binanceRestClient.SpotApi.Account.GetAccountInfoAsync(ct: cancellationToken)).CheckError();

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

		public async Task<decimal> GetTotalAccountBalance(CancellationToken cancellationToken=default)
		{
			var futuresBalance = GetAccountBalanceFutures(cancellationToken);
			var spotBalance = GetAccountBalanceSpot(cancellationToken);
			await Task.WhenAll(futuresBalance, spotBalance);
			var futuresAsset = futuresBalance.Result
				.FirstOrDefault(x => x.Asset == Domain.Defaults.TradingPlatform.DefaultAsset)?.WalletBalance??throw new NotFoundException("Futures Balance Not Found");
			var spotAsset = spotBalance.Result
				.FirstOrDefault(x => x.Asset == Domain.Defaults.TradingPlatform.DefaultAsset)?.Total??throw new NotFoundException("Spot Balance Not Found") ;
			return futuresAsset + spotAsset;
		}
	}
}
