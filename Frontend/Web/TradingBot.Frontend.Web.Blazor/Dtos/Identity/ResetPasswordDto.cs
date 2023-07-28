namespace TradingBot.Frontend.Web.Blazor.Dtos.Identity;

public class ResetPasswordDto
{
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}