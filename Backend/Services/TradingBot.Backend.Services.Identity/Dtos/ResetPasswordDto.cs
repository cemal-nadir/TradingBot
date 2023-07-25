namespace TradingBot.Backend.Services.Identity.Api.Dtos;

public class ResetPasswordDto
{
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}