namespace TradingBot.Frontend.Web.Blazor.Dtos.Users
{
	public class BalanceSettingsDto
	{
		public decimal CurrentBalance { get; set; }
		public decimal MinimumBalance { get; set; }
		public decimal AdjustBalancePercentage { get; set; }
		public decimal CurrentAdjustedBalance { get; set; }
		public int AdjustFrequencyDay { get; set; }
		public DateTime? LastAdjust { get; set; }
	}
}
