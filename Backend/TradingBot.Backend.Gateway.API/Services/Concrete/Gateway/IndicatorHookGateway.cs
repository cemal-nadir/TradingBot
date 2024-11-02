using CNG.Core.Exceptions;
using DotNetCore.CAP;
using TradingBot.Backend.Gateway.API.Dtos.Enums;
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
			await _tradingAccountService.AuthorizeFullAccess(cancellationToken);
			await _tradingHistoryService.AuthorizeFullAccess(cancellationToken);

			var tradingAccount = (await _tradingAccountService.GetByIndicatorIdAsync(indicatorId, cancellationToken)).CheckResponse();
			if (!tradingAccount.IsActive ||
				tradingAccount.Indicators?.FirstOrDefault(x => x.Id == indicatorId) is null ||
				!tradingAccount.Indicators.First(x => x.Id == indicatorId).IsActive) return;

			if (dto.Order is null) return;

			if (!Enum.TryParse(dto.Order.OrderType, out OrderType orderType))
				throw new NotFoundException($"{nameof(OrderType)} not found");

			var tradingHistory = (await _tradingHistoryService.GetLastOrder(dto.Order.Symbol ?? "",
				tradingAccount.Id ?? "", orderType, cancellationToken));

			await _capPublisher.PublishAsync(Defaults.Cap.BinanceHook, new HookModel()
			{
				IndicatorHook = dto,
				ApiKey = tradingAccount.ApiKey,
				SecretKey = tradingAccount.SecretKey,
				TradingHistoryId = tradingHistory.Data?.Id,
				CurrentAdjustedBalance = tradingAccount.BalanceSettings?.Plan?.CurrentAdjustedBalance??0,
				MinimumBalance = tradingAccount.BalanceSettings?.MinimumBalance??0,
				TradingHistoryQuantity = tradingHistory.Data?.Quantity??0,
				TradingAccountId = tradingAccount.Id,
				LossBasedPlan = tradingAccount.BalanceSettings?.Plan?.LossBased
			}, Defaults.Cap.HookResponse, cancellationToken: cancellationToken);
		}
	}
}
