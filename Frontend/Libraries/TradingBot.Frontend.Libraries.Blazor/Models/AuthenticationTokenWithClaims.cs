
namespace TradingBot.Frontend.Libraries.Blazor.Models
{
	public class AuthenticationTokenWithClaims
	{
		public string? AccessToken { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime Expired { get; set; }
		public AuthenticationClaims?Claims { get; set; }
	}
}
