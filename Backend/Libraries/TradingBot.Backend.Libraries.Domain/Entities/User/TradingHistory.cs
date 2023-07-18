using CNG.Abstractions.Signatures;
using MongoDB.Bson;
using TradingBot.Backend.Libraries.Domain.Enums;
using TradingBot.Backend.Libraries.Domain.Signatures;

namespace TradingBot.Backend.Libraries.Domain.Entities.User
{
	public class TradingHistory:UpdatedBase,IEntity<string>
	{
		public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
		public string? Symbol { get; set; }
		public Side Side { get; set; }
		public decimal Quantity { get; set; }
		public decimal EntryPrice { get; set; }
		public decimal ExitPrice { get; set; }
		public bool ClosedByBot { get; set; }
	}
}
