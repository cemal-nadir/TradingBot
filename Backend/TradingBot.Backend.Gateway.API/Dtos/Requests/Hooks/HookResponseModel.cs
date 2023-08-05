using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks
{
	public class HookResponseModel
	{
		public string? CloseTradingHistoryId { get; set; }
		public TradingHistoryDto? TradingHistory { get; set; }
		public decimal CurrentBalance { get; set; }
	}
}
