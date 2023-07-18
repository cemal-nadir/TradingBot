using CNG.Abstractions.Signatures;
using TradingBot.Backend.Gateway.API.Dtos.Enums;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Users
{
	public class TradingAccountDto:IDto
	{
		public string? UserId { get; set; }
		public string? Name { get; set; }
		public string? ApiKey { get; set; }
		public string? SecretKey { get; set; }
		public bool IsActive { get; set; }
		public BalanceSettingsDto? BalanceSettings { get; set; }
		public TradingPlatform Platform { get; set; }
		public List<IndicatorDto>? Indicators { get; set; }
		public List<string>? CurrentPositions { get; set; }
	}
}
