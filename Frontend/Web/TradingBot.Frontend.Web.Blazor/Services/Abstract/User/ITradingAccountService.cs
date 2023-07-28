using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;

namespace TradingBot.Frontend.Web.Blazor.Services.Abstract.User
{
	public interface ITradingAccountService:IServiceRepository<string,TradingAccountDto,TradingAccountsDto>
	{
	}
}
