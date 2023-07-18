using CNG.Abstractions.Signatures;
using TradingBot.Backend.Gateway.API.Dtos.Enums;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Users
{
	public class TradingHistoryDto:IDto
	{
		public string? Symbol { get; set; }
		public Side Side { get; set; }
		public decimal Quantity { get; set; }
		public decimal EntryPrice { get; set; }
		public decimal ExitPrice { get; set; }
		public bool ClosedByBot { get; set; }
	}
}
