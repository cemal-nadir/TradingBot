using TradingBot.Frontend.Libraries.Blazor.Signatures;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Users
{
	public class TradingAccountsDto:TradingAccountDto,IListDto<string>
	{
		public string Id { get; set; }
	}
}
