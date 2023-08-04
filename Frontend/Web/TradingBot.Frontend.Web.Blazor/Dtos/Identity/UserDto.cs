using FluentValidation;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Libraries.Blazor.Signatures;
using TradingBot.Frontend.Web.Blazor.Dtos.Enums;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Identity;

public class UserDto:IDto
{
	public string? UserName { get; set; }
	public string? Email { get; set; }
	public bool IsConfirmed { get; set; }
	public string? Name { get; set; }
	public string? SurName { get; set; }
	public Gender Gender { get; set; }
	public DateTime? BirthDate { get; set; }
	public List<string>? Roles { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Password { get; set; }
}

public class UserValidator : ValidatorBase<UserDto>
{
	public UserValidator()
	{
		RuleFor(x => x.BirthDate).NotNull();
		RuleFor(x => x.Email).NotEmpty().EmailAddress().MinimumLength(7).MaximumLength(256);
		RuleFor(x => x.Gender).IsInEnum();
		RuleFor(x => x.Name).NotEmpty().MinimumLength(1).MaximumLength(200);
		RuleFor(x => x.SurName).NotNull().MinimumLength(1).MaximumLength(200);
		RuleFor(x => x.IsConfirmed).NotNull();
		RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(32);
		RuleFor(x => x.PhoneNumber).NotEmpty().MinimumLength(10).MaximumLength(15);
	}
}
