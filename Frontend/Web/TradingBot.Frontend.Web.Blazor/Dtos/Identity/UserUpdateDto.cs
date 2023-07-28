using TradingBot.Frontend.Web.Blazor.Dtos.Enums;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Identity;

public class UserUpdateDto
{
	public string? Id { get; set; }
	public string? Name { get; set; }
	public string? SurName { get; set; }
	public DateTime? UserBirthDate { get; set; }
	public Gender? Gender { get; set; }
	public List<string>? Roles { get; set; }
	public string? UserName { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Password { get; set; }
}