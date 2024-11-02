using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks
{
	public class HookModel
	{
		public string? TradingHistoryId { get; set; }
		public string? TradingAccountId { get; set; }
		public decimal? TradingHistoryQuantity { get; set; }
		public decimal CurrentAdjustedBalance { get; set; }
		public decimal MinimumBalance { get; set; }
		public string? ApiKey { get; set; }
		public string? SecretKey { get; set; }
		public IndicatorHookDto? IndicatorHook { get; set; }
		public TradingAccountDto.BalanceDetailDto.BudgetPlanDto.LossBasedPlanDto? LossBasedPlan { get; set; }
	}
}
