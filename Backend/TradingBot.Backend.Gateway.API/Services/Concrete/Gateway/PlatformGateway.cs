using CNG.Core.Exceptions;
using TradingBot.Backend.Gateway.API.Dtos.Enums;
using TradingBot.Backend.Gateway.API.Extensions;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Gateway
{
	public class PlatformGateway : IPlatformGateway
	{
		private readonly IBinanceAccountService _binanceAccountService;

		public PlatformGateway(IBinanceAccountService binanceAccountService)
		{
			_binanceAccountService = binanceAccountService;
		}

		private const string BinanceMainAsset = "USDT";

		public async Task<decimal> GetCurrentBalance(string apiKey, string secretKey, string platform, CancellationToken cancellationToken = default)
		{
			if (!Enum.TryParse(typeof(TradingPlatform), platform, out var platformEnum))
				throw new NotFoundException(Defaults.Error.NotFound.TradingPlatformNotFound);

			switch ((TradingPlatform)platformEnum)
			{
				case TradingPlatform.Binance:
				default:
					var spotBalances =
						_binanceAccountService.GetAccountBalanceSpotAsync(apiKey, secretKey, cancellationToken);
					var futuresBalances =
						_binanceAccountService.GetAccountBalanceFuturesAsync(apiKey, secretKey, cancellationToken);
					await Task.WhenAll(spotBalances, futuresBalances);

					return (spotBalances.Result.CheckResponse()?.FirstOrDefault(x => x.Asset == BinanceMainAsset)
						?.Total ?? 0) + (futuresBalances.Result.CheckResponse()
						?.FirstOrDefault(x => x.Asset == BinanceMainAsset)?.WalletBalance ?? 0);
			}
		}
	}
}
