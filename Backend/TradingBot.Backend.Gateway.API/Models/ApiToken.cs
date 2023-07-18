using TradingBot.Backend.Gateway.API.Dtos.Enums;

namespace TradingBot.Backend.Gateway.API.Models
{
	public class ApiToken
	{
		public AuthType Type { get; set; }
		public string? Id { get; set; }
		public string? Secret { get; set; }
		public string? Token { get; set; }
		public DateTime ExpireTime { get; set; }
	}
}
