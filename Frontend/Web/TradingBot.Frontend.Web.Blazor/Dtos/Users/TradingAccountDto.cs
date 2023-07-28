using TradingBot.Frontend.Libraries.Blazor.Signatures;
using TradingBot.Frontend.Web.Blazor.Dtos.Enums;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Users
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
	}
}
