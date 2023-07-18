using CNG.Abstractions.Repositories;
using TradingBot.Backend.Libraries.Application.Dtos.User;

namespace TradingBot.Backend.Libraries.Application.Services.User
{
	public interface ITradingAccountService:IServiceRepository<TradingAccountDto,string>
	{
		Task<TradingAccountsDto> GetByIndicatorId(string indicatorId, CancellationToken cancellationToken = default);
	}
}
