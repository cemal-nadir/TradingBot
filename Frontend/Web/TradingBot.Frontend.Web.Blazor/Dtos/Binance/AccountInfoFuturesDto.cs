namespace TradingBot.Frontend.Web.Blazor.Dtos.Binance
{
	public class AccountInfoFuturesDto
	{

		public List<AssetDto>? Assets { get; set; }
		public bool CanDeposit { get; set; }
		public bool CanTrade { get; set; }
		public bool CanWithdraw { get; set; }
		public decimal FeeTier { get; set; }
		public decimal MaxWithdrawQuantity { get; set; }
		public List<PositionDto>? Positions { get; set; }
		public decimal TotalInitialMargin { get; set; }
		public decimal TotalMaintMargin { get; set; }
		public decimal TotalMarginBalance { get; set; }
		public decimal TotalOpenOrderInitialMargin { get; set; }
		public decimal TotalPositionInitialMargin { get; set; }
		public decimal TotalUnrealizedProfit { get; set; }
		public decimal TotalWalletBalance { get; set; }
		public decimal TotalCrossWalletBalance { get; set; }
		public decimal TotalCrossUnPnl { get; set; }
		public decimal AvailableBalance { get; set; }
		public DateTime UpdateTime { get; set; }
	}
	public class AssetDto
	{
		public string? Asset { get; set; }
		public decimal InitialMargin { get; set; }
		public decimal MaintMargin { get; set; }
		public decimal MarginBalance { get; set; }
		public decimal MaxWithdrawQuantity { get; set; }
		public decimal OpenOrderInitialMargin { get; set; }
		public decimal PositionInitialMargin { get; set; }
		public decimal UnrealizedPnl { get; set; }
		public decimal WalletBalance { get; set; }
		public decimal CrossWalletBalance { get; set; }
		public decimal CrossUnrealizedPnl { get; set; }
		public decimal AvailableBalance { get; set; }
		public bool MarginAvailable { get; set; }
		public DateTime? UpdateTime { get; set; }
	}
	public class PositionDto
	{
		public decimal MaxNotional { get; set; }
		public decimal İnitialMargin { get; set; }
		public decimal MaintMargin { get; set; }
		public decimal PositionInitialMargin { get; set; }
		public decimal OpenOrderInitialMargin { get; set; }
		public bool İsolated { get; set; }
		public decimal Quantity { get; set; }
		public DateTime UpdateTime { get; set; }
		public string? Symbol { get; set; }
		public decimal EntryPrice { get; set; }
		public decimal Leverage { get; set; }
		public decimal UnrealizedPnl { get; set; }
		public decimal PositionSide { get; set; }
	}

}
