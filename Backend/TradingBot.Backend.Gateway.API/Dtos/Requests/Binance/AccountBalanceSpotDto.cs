namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Binance
{
	public class AccountBalanceSpotDto
	{
		public decimal Freeze { get; set; }
		public decimal Withdrawing { get; set; }
		public decimal Ipoable { get; set; }
		public decimal BtcValuation { get; set; }
		public string? Asset { get; set; }
		public decimal Available { get; set; }
		public decimal Locked { get; set; }
		public decimal Total { get; set; }

	}
}
