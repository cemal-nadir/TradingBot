using TradingBot.Backend.Libraries.Application.Dtos.User;

namespace TradingBot.Backend.Services.Binance.API.Models
{
	public class HookModel
	{
		public IndicatorHook? IndicatorHook { get; set; }
		public TradingAccountsDto? Account { get; set; }
	}
}
