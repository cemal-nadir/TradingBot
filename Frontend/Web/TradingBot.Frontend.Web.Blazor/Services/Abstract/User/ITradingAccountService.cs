using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;

namespace TradingBot.Frontend.Web.Blazor.Services.Abstract.User
{
	public interface ITradingAccountService:IServiceRepository<string,TradingAccountDto,TradingAccountsDto>
	{
		Task<Response<List<TradingAccountsDto>>> GetTradingAccountsByUserId(string userId,
			CancellationToken cancellationToken = default);
	}
}
