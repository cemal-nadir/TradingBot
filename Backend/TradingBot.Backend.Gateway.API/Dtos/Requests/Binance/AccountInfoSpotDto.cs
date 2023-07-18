namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Binance
{
	public class AccountInfoSpotDto
	{
		public decimal MakerFee { get; set; }
		public decimal TakerFee { get; set; }
		public decimal BuyerFee { get; set; }
		public decimal SellerFee { get; set; }
		public bool CanTrade { get; set; }
		public bool CanWithdraw { get; set; }
		public bool CanDeposit { get; set; }
		public bool Brokered { get; set; }
		public DateTime UpdateTime { get; set; }
		public decimal AccountType { get; set; }
		public List<BalanceDto>? Balances { get; set; }
	}
	public class BalanceDto
	{
		public string? Asset { get; set; }
		public decimal Available { get; set; }
		public decimal Locked { get; set; }
		public decimal Total { get; set; }
	}
}
