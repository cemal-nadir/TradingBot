using FluentValidation;
using TradingBot.Frontend.Libraries.Blazor.Services;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Account
{
	public class LoginDto
	{
		public string? Email { get; set; }
		public string? Password { get; set; }
	}

	public class LoginValidator : ValidatorBase<LoginDto>
	{
		public LoginValidator()
		{
			RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(32);
			RuleFor(x => x.Email).NotEmpty().MinimumLength(7).MaximumLength(256).EmailAddress();
		}
	}
}
