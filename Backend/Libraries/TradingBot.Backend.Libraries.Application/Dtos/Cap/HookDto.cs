using TradingBot.Backend.Libraries.Application.Dtos.User;

namespace TradingBot.Backend.Libraries.Application.Dtos.Cap
{
	public class HookDto
	{
		public string? TradingAccountId { get; set; }
		public string? TradingHistoryId { get; set; }
		public decimal? TradingHistoryQuantity { get; set; }
		public decimal CurrentAdjustedBalance { get; set; }
		public decimal MinimumBalance { get; set; }
		public string? ApiKey { get; set; }
		public string? SecretKey { get; set; }
		public IndicatorHookDto? IndicatorHook { get; set; }
        public TradingAccountDto.BalanceDetailDto.BudgetPlanDto.LossBasedPlanDto? LossBasedPlan { get; set; }
    }
}
