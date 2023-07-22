using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks
{
	public class HookModel
	{
		public TradingHistoriesDto? TradingHistory { get; set; }
		public IndicatorHookDto? IndicatorHook { get; set; }
		public TradingAccountsDto? Account { get; set; }
	}
}
