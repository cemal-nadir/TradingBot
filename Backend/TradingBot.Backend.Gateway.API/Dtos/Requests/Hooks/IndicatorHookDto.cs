using TradingBot.Backend.Gateway.API.Dtos.Enums;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks
{
	public class IndicatorHookDto
	{
		public Order? Order { get; set; }
		public decimal? TakeProfitPercentage { get; set; }
		public decimal? StopLossPercentage { get; set; }
		public TrailingStopLoss? TrailingStopLoss { get; set; }
	}

	public class TrailingStopLoss
	{
		public decimal CallBackPercentage { get; set; }
		public decimal ActivationPercentage { get; set; }
	}

	public class Order
	{
		public string? Symbol { get; set; }
		public Side? Side { get; set; }
		public OrderType OrderType { get; set; }
		public MarginType MarginType { get; set; }
		public int Leverage { get; set; }
		public bool? ClosePosition { get; set; }

	}
}
