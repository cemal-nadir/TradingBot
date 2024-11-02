using FluentValidation;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Libraries.Blazor.Signatures;
using TradingBot.Frontend.Web.Blazor.Dtos.Enums;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Users
{
	public class TradingAccountDto:IDto
	{
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? ApiKey { get; set; }
        public string? SecretKey { get; set; }
        public bool IsActive { get; set; }
        public TradingPlatform Platform { get; set; }

        public BalanceDetailDto BalanceSettings { get; set; } = new();
        public List<IndicatorDto>? Indicators { get; set; }

        public class BalanceDetailDto
        {
            public decimal CurrentBalance { get; set; }
            public decimal MinimumBalance { get; set; }
            public BudgetPlanDto Plan { get; set; } = new();
            public class BudgetPlanDto
            {
                public decimal AdjustBalancePercentage { get; set; }
                public decimal CurrentAdjustedBalance { get; set; }
                public int AdjustFrequencyDay { get; set; }
                public DateTime? LastAdjust { get; set; }
                public LossBasedPlanDto? LossBased { get; set; }
                public class LossBasedPlanDto
                {
                    public decimal LossAmountPercentage { get; set; }
                    public decimal? StopLossPercentage { get; set; }
                }
            }
        }
    }
    public class TradingAccountValidator : ValidatorBase<TradingAccountDto>
    {
        public TradingAccountValidator()
        {
            RuleFor(x => x.ApiKey).NotEmpty().MinimumLength(32).MaximumLength(256);
            RuleFor(x => x.SecretKey).NotEmpty().MinimumLength(32).MaximumLength(256);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.Platform).IsInEnum();
            RuleFor(x => x.BalanceSettings.Plan.AdjustBalancePercentage).NotEmpty().LessThanOrEqualTo(99)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.BalanceSettings.Plan.AdjustFrequencyDay).NotEmpty().LessThanOrEqualTo(365)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.BalanceSettings.MinimumBalance).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
