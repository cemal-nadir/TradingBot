using CNG.Core.Exceptions;
using DotNetCore.CAP;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;

namespace TradingBot.Backend.Gateway.API.Caps
{
	public class PullSubscribeService : ICapSubscribe
	{
		private readonly ITradingHistoryService _tradingHistoryService;

		public PullSubscribeService(ITradingHistoryService tradingHistoryService)
		{
			_tradingHistoryService = tradingHistoryService;
		}

		[CapSubscribe(Defaults.Cap.HookResponse)]
		public async Task HookResponse(HookResponseModel hookResponse, CancellationToken cancellationToken = default)
		{
			if (hookResponse.TradingHistory is null) return;
			var closeTradingHistory = await _tradingHistoryService.GetAsync(hookResponse.CloseTradingHistoryId ?? "", cancellationToken);
			if (!closeTradingHistory.Success) throw new BadRequestException(closeTradingHistory.Message);

			if (closeTradingHistory.Data != null)
			{
				var data = closeTradingHistory.Data;
				data.IsClosed = true;
				await _tradingHistoryService.UpdateAsync(hookResponse.CloseTradingHistoryId ?? "", data, cancellationToken);
			}

			await _tradingHistoryService.InsertAsync(hookResponse.TradingHistory, cancellationToken);
		}
	}
}
