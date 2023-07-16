namespace TradingBot.Backend.Libraries.ApiCore.Responses;

public class TokenResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expired { get; set; }
}