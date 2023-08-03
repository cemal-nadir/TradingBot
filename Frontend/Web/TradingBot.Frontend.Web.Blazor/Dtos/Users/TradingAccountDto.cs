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
        public BalanceSettingsDto BalanceSettings { get; set; } = new();
		public TradingPlatform Platform { get; set; }
        public List<IndicatorDto> Indicators { get; set; } = new();
    }
    public class TradingAccountValidator : ValidatorBase<TradingAccountDto>
    {
        public TradingAccountValidator()
        {
            RuleFor(x => x.ApiKey).NotEmpty().MinimumLength(32).MaximumLength(256);
            RuleFor(x => x.SecretKey).NotEmpty().MinimumLength(32).MaximumLength(256);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.Platform).IsInEnum();
            RuleFor(x => x.BalanceSettings.AdjustFrequencyDay).NotEmpty().LessThanOrEqualTo(365).GreaterThanOrEqualTo(1);
            RuleFor(x => x.BalanceSettings.AdjustBalancePercentage).NotEmpty().LessThanOrEqualTo(99)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.BalanceSettings.MinimumBalance).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}
