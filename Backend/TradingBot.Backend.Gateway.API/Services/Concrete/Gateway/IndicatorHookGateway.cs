using DotNetCore.CAP;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks;
using TradingBot.Backend.Gateway.API.Extensions;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Gateway
{

	public class IndicatorHookGateway : IIndicatorHookGateway
	{
		private readonly ITradingAccountService _tradingAccountService;
		private readonly ITradingHistoryService _tradingHistoryService;
		private readonly ICapPublisher _capPublisher;
		public IndicatorHookGateway(ITradingAccountService tradingAccountService, ICapPublisher capPublisher, ITradingHistoryService tradingHistoryService)
		{
			_tradingAccountService = tradingAccountService;
			_capPublisher = capPublisher;
			_tradingHistoryService = tradingHistoryService;
		}

		public async Task HookAsync(string indicatorId, IndicatorHookDto dto, CancellationToken cancellationToken = default)
		{
			var tradingAccount = (await _tradingAccountService.GetByIndicatorIdAsync(indicatorId, cancellationToken)).CheckResponse();
			if (!tradingAccount.IsActive ||
				tradingAccount.Indicators?.FirstOrDefault(x => x.Id == indicatorId) is null ||
				!tradingAccount.Indicators.First(x => x.Id == indicatorId).IsActive) return;

			if (dto.Order is null) return;


			var tradingHistory = await _tradingHistoryService.GetLastOrder(dto.Order.Symbol ?? "",
				tradingAccount.Id ?? "", dto.Order.OrderType, cancellationToken);

			await _capPublisher.PublishAsync(Defaults.Cap.BinanceHook, new HookModel()
			{
				IndicatorHook = dto,
				Account = tradingAccount,
				TradingHistory = tradingHistory.Data
			}, cancellationToken: cancellationToken);
		}
	}
}
