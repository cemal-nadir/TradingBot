using TradingBot.Frontend.Libraries.Blazor.Signatures;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Users
{
	public class TradingHistoriesDto:TradingHistoryDto,IListDto<string>
	{
		public string Id { get; set; }
	}
}
