using DotNetCore.CAP;
using TradingBot.Backend.Libraries.Domain.Defaults;
using TradingBot.Backend.Libraries.Application.Dtos.Cap;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Services.User;
using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Services.User.API.Caps
{
	public class PullSubscribeService : ICapSubscribe
	{
		private readonly ITradingHistoryService _tradingHistoryService;
		private readonly ITradingAccountService _tradingAccountService;

		public PullSubscribeService(ITradingHistoryService tradingHistoryService, ITradingAccountService tradingAccountService)
		{
			_tradingHistoryService = tradingHistoryService;
			_tradingAccountService = tradingAccountService;
		}
		[CapSubscribe(Cap.HookResponse)]
		public async Task HookResponse(HookResponseDto hookResponse, CancellationToken cancellationToken = default)
		{
			if (hookResponse.TradingAccountId is null) return;

			if (hookResponse.CloseTradingHistoryId != null)
			{
				var closeTradingHistory = await _tradingHistoryService.GetAsync(hookResponse.CloseTradingHistoryId, cancellationToken);
				closeTradingHistory.IsClosed = true;
				await _tradingHistoryService.UpdateAsync(hookResponse.CloseTradingHistoryId, closeTradingHistory, cancellationToken);
			}


			if (hookResponse.TradingHistory != null && Enum.TryParse(hookResponse.TradingHistory.OrderType, out OrderType orderType) && Enum.TryParse(hookResponse.TradingHistory.Side, out Side side))
			{
				await _tradingHistoryService.InsertAsync(new TradingHistoryDto
				{
					IsClosed = false,
					EntryPrice = hookResponse.TradingHistory.EntryPrice,
					Quantity = hookResponse.TradingHistory.Quantity,
					Symbol = hookResponse.TradingHistory.Symbol,
					OrderType = orderType,
					Side = side

				}, cancellationToken);
			}

			var acc = await _tradingAccountService.GetAsync(hookResponse.TradingAccountId, cancellationToken);
			
			if (acc.BalanceSettings != null)
			{
				acc.BalanceSettings.CurrentBalance = hookResponse.CurrentBalance;
				await _tradingAccountService.UpdateAsync(hookResponse.TradingAccountId, acc, cancellationToken);
			}
		}
	}
}
