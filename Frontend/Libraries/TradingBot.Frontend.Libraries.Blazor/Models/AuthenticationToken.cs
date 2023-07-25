namespace TradingBot.Frontend.Libraries.Blazor.Models
{
	public class AuthenticationToken
	{
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime Expired { get; set; }
	}
}
