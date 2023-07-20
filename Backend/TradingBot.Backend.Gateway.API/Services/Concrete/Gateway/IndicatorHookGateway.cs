using CNG.Core.Exceptions;
using DotNetCore.CAP;
using TradingBot.Backend.Gateway.API.Dtos.Enums;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks;
using TradingBot.Backend.Gateway.API.Extensions;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Gateway
{

	public class IndicatorHookGateway : IIndicatorHookGateway
	{
		private readonly ITradingAccountService _tradingAccountService;
		private readonly IBinanceAccountService _binanceAccountService;
		private readonly ICapPublisher _capPublisher;
		private const string BinanceMainAsset = "USDT";
		public IndicatorHookGateway(ITradingAccountService tradingAccountService, IBinanceAccountService binanceAccountService, ICapPublisher capPublisher)
		{
			_tradingAccountService = tradingAccountService;
			_binanceAccountService = binanceAccountService;
			_capPublisher = capPublisher;
		}

		public async Task HookAsync(string indicatorId, IndicatorHookDto dto, CancellationToken cancellationToken = default)
		{
			var tradingAccount = (await _tradingAccountService.GetByIndicatorIdAsync(indicatorId, cancellationToken)).CheckResponse();
			if (!tradingAccount.IsActive ||
				tradingAccount.Indicators?.FirstOrDefault(x => x.Id == indicatorId) is null ||
				!tradingAccount.Indicators.First(x => x.Id == indicatorId).IsActive) return;

			switch (tradingAccount.Platform)
			{
				case TradingPlatform.Binance:
				default:
					switch (dto.Order?.OrderType)
					{
						case OrderType.Futures:
						default:
						{
							var balance = (await _binanceAccountService.GetAccountBalanceFuturesAsync(
								              tradingAccount.ApiKey ?? "",
								              tradingAccount.SecretKey ?? "", cancellationToken)).CheckResponse()
							              ?.FirstOrDefault(x => x.Asset == BinanceMainAsset) ??
							              throw new NotFoundException("Balance not found");

							if (balance.WalletBalance < tradingAccount.BalanceSettings?.MinimumBalance ||
							    balance.AvailableBalance < tradingAccount.BalanceSettings?.CurrentAdjustedBalance)
								return;

							break;
						}
						case OrderType.Spot:
						{
							var balance = (await _binanceAccountService.GetAccountBalanceSpotAsync(
								              tradingAccount.ApiKey ?? "",
								              tradingAccount.SecretKey ?? "", cancellationToken)).CheckResponse()
							              ?.FirstOrDefault(x => x.Asset == BinanceMainAsset) ??
							              throw new NotFoundException("Balance not found");
							if (balance.Total < tradingAccount.BalanceSettings?.MinimumBalance ||
							    balance.Available < tradingAccount.BalanceSettings?.CurrentAdjustedBalance)
								return;
							break;
						}
					}
					break;

			}

		}
	}
}
