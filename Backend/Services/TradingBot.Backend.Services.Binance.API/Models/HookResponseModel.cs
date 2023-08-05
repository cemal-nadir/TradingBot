using TradingBot.Backend.Libraries.Application.Dtos.User;

namespace TradingBot.Backend.Services.Binance.API.Models
{
	public class HookResponseModel
	{
		public string? CloseTradingHistoryId { get; set; }
		public TradingHistoryDto? TradingHistory { get; set; }
		public decimal CurrentBalance { get; set; }
	}
}
