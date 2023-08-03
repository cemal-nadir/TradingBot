namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Binance
{
	public class AccountBalanceFuturesDto
	{
		public decimal MaxWithdrawQuantity { get; set; }
		public bool MarginAvailable { get; set; }
		public string? AccountAlias { get; set; }
		public string? Asset { get; set; }
		public decimal WalletBalance { get; set; }
		public decimal CrossWalletBalance { get; set; }
		public decimal CrossUnrealizedPnl { get; set; }
		public decimal AvailableBalance { get; set; }
		public DateTime UpdateTime { get; set; }

	}
}
