using TradingBot.Frontend.Libraries.Blazor.Signatures;
using TradingBot.Frontend.Web.Blazor.Dtos.Enums;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Users
{
	public class TradingHistoryDto:IDto
	{
		public string? TradingAccountId { get; set; }
		public string? Symbol { get; set; }
		public Side Side { get; set; }
		public OrderType OrderType { get; set; }
		public decimal Quantity { get; set; }
		public decimal EntryPrice { get; set; }
		public bool IsClosed { get; set; }
	}
}
