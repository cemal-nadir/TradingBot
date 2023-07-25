using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Repositories;
using TradingBot.Backend.Gateway.API.Responses;

namespace TradingBot.Backend.Gateway.API.Services.Abstract.Api.User
{
	public interface ITradingAccountService : IServiceRepository<string, TradingAccountDto, TradingAccountsDto>
	{

		Task<Response<List<TradingAccountsDto>>> GetAllByUserIdAsync(string userId,
			CancellationToken cancellationToken = default);
		Task<Response<TradingAccountsDto>> GetByIndicatorIdAsync(string indicatorId,
			CancellationToken cancellationToken = default);
	}
}
