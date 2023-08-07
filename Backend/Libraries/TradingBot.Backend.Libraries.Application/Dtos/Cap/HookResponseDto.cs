
namespace TradingBot.Backend.Libraries.Application.Dtos.Cap
{
	public class HookResponseDto
	{
		public string? CloseTradingHistoryId { get; set; }
		public HookResponseTradingHistory? TradingHistory { get; set; }
		public decimal CurrentBalance { get; set; }
		public string? TradingAccountId { get; set; }
	}

	public class HookResponseTradingHistory
	{
		public string? Symbol { get; set; }
		public string? Side { get; set; }
		public string? OrderType { get; set; }
		public decimal Quantity { get; set; }
		public decimal EntryPrice { get; set; }
	}
}
