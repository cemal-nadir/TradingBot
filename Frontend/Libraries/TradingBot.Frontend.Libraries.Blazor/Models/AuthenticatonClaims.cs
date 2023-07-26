using Newtonsoft.Json;
namespace TradingBot.Frontend.Libraries.Blazor.Models
{
	public class AuthenticationClaims
	{
		[JsonProperty(PropertyName = "sub")]
		public string? Sub { get; set; }
		[JsonProperty(PropertyName = "role")]
		public string? Role { get; set; }
		[JsonProperty(PropertyName = "preferred_username")]
		public string? PreferredUsername { get; set; }
		[JsonProperty(PropertyName = "name")]
		public string? Name { get; set; }
		[JsonProperty(PropertyName = "email")]
		public string? Email { get; set; }
		[JsonProperty(PropertyName = "email_verified")]
		public bool EmailVerified { get; set; }
		[JsonProperty(PropertyName = "phone_number")]
		public string? PhoneNumber { get; set; }
		[JsonProperty(PropertyName = "phone_number_verified")]
		public bool PhoneNumberVerified { get; set; }
	}
}
