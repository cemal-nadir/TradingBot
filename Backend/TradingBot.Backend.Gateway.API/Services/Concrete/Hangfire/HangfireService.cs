using DotNetCore.CAP;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Hangfire;
using TradingBot.Backend.Gateway.API.Services.Concrete.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Hangfire
{
	public class HangfireService : IHangfireService
	{
		private readonly IBinanceAccountService _binanceAccountService;
		private readonly ITradingAccountService _tradingAccountService;

		public HangfireService(IBinanceAccountService binanceAccountService, ITradingAccountService tradingAccountService)
		{
			_binanceAccountService = binanceAccountService;
			_tradingAccountService = tradingAccountService;
			_tradingAccountService.AuthorizeFullAccess().Wait();
		}
		public async Task AdjustAllBalances(CancellationToken cancellationToken = default)
		{
			var accounts = await _tradingAccountService.GetAllAsync(cancellationToken);
			if (!accounts.Success || accounts.Data is null) return;
			List<Task> accountTasks = (from account in accounts.Data.Where(x => x.IsActive)
									   where account.BalanceSettings?.LastAdjust == null || account.BalanceSettings.LastAdjust.Value.AddDays(account.BalanceSettings.AdjustFrequencyDay) <= DateTime.Now
									   select Task.Run(async () =>
									   {
										   var spotBalanceTask = _binanceAccountService.GetAccountBalanceSpotAsync(account.ApiKey ?? "", account.SecretKey ?? "", cancellationToken);
										   var futureBalanceTask = _binanceAccountService.GetAccountBalanceFuturesAsync(account.ApiKey ?? "", account.SecretKey ?? "", cancellationToken);
										   await Task.WhenAll(spotBalanceTask, futureBalanceTask);


										   if (!spotBalanceTask.Result.Success || !futureBalanceTask.Result.Success || spotBalanceTask.Result.Data?.FirstOrDefault(x => x.Asset == "USDT")?.Total is null || futureBalanceTask.Result.Data?.FirstOrDefault(x => x.Asset == "USDT")?.WalletBalance is null) return;

										   var balance = spotBalanceTask.Result.Data?.FirstOrDefault(x => x.Asset == "USDT")?.Total + (futureBalanceTask.Result.Data?.FirstOrDefault(x => x.Asset == "USDT")?.WalletBalance);
										   if (balance is null) return;

										   if (account.BalanceSettings == null) return;

										   account.BalanceSettings.CurrentBalance = balance.Value;
										   account.BalanceSettings.LastAdjust = DateTime.Now;
										   account.BalanceSettings.CurrentAdjustedBalance = (balance.Value * account.BalanceSettings.AdjustBalancePercentage) / 100;

										   await _tradingAccountService.UpdateAsync(account.Id ?? "", account, cancellationToken);
									   }, cancellationToken)).ToList();

			await Task.WhenAll(accountTasks);
		}

	}
}
