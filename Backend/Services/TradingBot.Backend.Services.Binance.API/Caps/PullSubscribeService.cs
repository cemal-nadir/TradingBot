using CNG.Core.Exceptions;
using DotNetCore.CAP;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;
using TradingBot.Backend.Libraries.Domain.Defaults;
using TradingBot.Backend.Libraries.Domain.Enums;
using TradingBot.Backend.Services.Binance.API.Models;

namespace TradingBot.Backend.Services.Binance.API.Caps
{
	public class PullSubscribeService:ICapSubscribe
	{
		private readonly IBinanceMainClient _binanceMainClient;
		private const string BinanceMainAsset = "USDT";
		public PullSubscribeService(IBinanceMainClient binanceMainClient)
		{
			_binanceMainClient = binanceMainClient;
		}

		[CapSubscribe(Cap.BinanceHook)]
		public async Task HookAsync(HookModel model, CancellationToken cancellationToken = default)
		{
			_binanceMainClient.SetClient(model.Account?.ApiKey??"",model.Account?.SecretKey??"");

			switch (model.IndicatorHook?.Order?.OrderType)
			{
				case OrderType.Futures:
				default:
				{
					


							var balance =
						(await _binanceMainClient.BinanceAccountService.GetAccountBalanceFutures(cancellationToken))
						.FirstOrDefault(x => x.Asset == BinanceMainAsset) ??
						throw new NotFoundException("Balance not found");

					if (balance.WalletBalance < model.Account?.BalanceSettings?.MinimumBalance ||
					    balance.AvailableBalance < model.Account?.BalanceSettings?.CurrentAdjustedBalance)
						return;


					

					break;
				}
				case OrderType.Spot:
				{
					var balance = (await _binanceMainClient.BinanceAccountService.GetAccountBalanceSpot(
						            cancellationToken)).FirstOrDefault(x => x.Asset == BinanceMainAsset) ??
					              throw new NotFoundException("Balance not found");
					if (balance.Total < model.Account?.BalanceSettings?.MinimumBalance ||
					    balance.Available < model.Account?.BalanceSettings?.CurrentAdjustedBalance)
						return;
					break;
				}
			}
		}
	}
}
