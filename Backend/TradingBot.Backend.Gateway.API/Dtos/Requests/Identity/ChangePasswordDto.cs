namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;

public class ChangePasswordDto
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;

}