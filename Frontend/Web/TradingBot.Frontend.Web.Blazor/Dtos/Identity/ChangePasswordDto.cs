namespace TradingBot.Frontend.Web.Blazor.Dtos.Identity;

public class ChangePasswordDto
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;

}