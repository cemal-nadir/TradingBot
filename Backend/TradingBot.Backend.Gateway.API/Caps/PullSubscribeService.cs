using CNG.Core.Exceptions;
using DotNetCore.CAP;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;

namespace TradingBot.Backend.Gateway.API.Caps
{
	public class PullSubscribeService : ICapSubscribe
	{
		private readonly ITradingHistoryService _tradingHistoryService;
		private readonly ITradingAccountService _tradingAccountService;

		public PullSubscribeService(ITradingHistoryService tradingHistoryService, ITradingAccountService tradingAccountService)
		{
			_tradingHistoryService = tradingHistoryService;
			_tradingAccountService = tradingAccountService;
			_tradingAccountService.AuthorizeFullAccess().RunSynchronously();
			_tradingHistoryService.AuthorizeFullAccess().RunSynchronously();
		}

		[CapSubscribe(Defaults.Cap.HookResponse)]
		public async Task HookResponse(HookResponseModel hookResponse, CancellationToken cancellationToken = default)
		{
			if (hookResponse.CloseTradingHistoryId is null) return;
			var closeTradingHistory = await _tradingHistoryService.GetAsync(hookResponse.CloseTradingHistoryId ?? "", cancellationToken);
			if (!closeTradingHistory.Success) throw new BadRequestException(closeTradingHistory.Message);

			if (closeTradingHistory.Data != null)
			{
				var data = closeTradingHistory.Data;
				data.IsClosed = true;
				await _tradingHistoryService.UpdateAsync(hookResponse.CloseTradingHistoryId ?? "", data, cancellationToken);
			}
			if (hookResponse.TradingHistory != null)
				await _tradingHistoryService.InsertAsync(hookResponse.TradingHistory, cancellationToken);

			var acc = await _tradingAccountService.GetAsync(closeTradingHistory.Data?.TradingAccountId ?? "", cancellationToken);
			var accountData = acc.Data;
			if (accountData?.BalanceSettings != null)
			{
				accountData.BalanceSettings.CurrentBalance = hookResponse.CurrentBalance;
				await _tradingAccountService.UpdateAsync(closeTradingHistory.Data?.TradingAccountId ?? "", accountData, cancellationToken);
			}
		}
	}
}
