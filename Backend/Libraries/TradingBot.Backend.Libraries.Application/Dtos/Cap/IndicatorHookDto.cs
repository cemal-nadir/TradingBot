namespace TradingBot.Backend.Libraries.Application.Dtos.Cap;

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
	public string? Side { get; set; }
	public string? OrderType { get; set; }
	public string? MarginType { get; set; }
	public int? Leverage { get; set; }
	public bool? ClosePosition { get; set; }
}