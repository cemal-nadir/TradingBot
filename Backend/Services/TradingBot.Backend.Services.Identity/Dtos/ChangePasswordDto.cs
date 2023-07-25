namespace TradingBot.Backend.Services.Identity.Api.Dtos;

public class ChangePasswordDto
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;

}