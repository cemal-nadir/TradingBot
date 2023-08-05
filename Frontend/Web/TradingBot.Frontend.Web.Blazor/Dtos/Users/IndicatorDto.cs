using FluentValidation;
using TradingBot.Frontend.Libraries.Blazor.Services;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Users
{
	public class IndicatorDto
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public bool IsActive { get; set; }
	}

	public class IndicatorValidator : ValidatorBase<IndicatorDto>
	{
		public IndicatorValidator()
		{
			RuleFor(x => x.IsActive).NotNull();
			RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
			RuleFor(x => x.Description).NotEmpty().MinimumLength(5).MaximumLength(200);
		}
	}
}
