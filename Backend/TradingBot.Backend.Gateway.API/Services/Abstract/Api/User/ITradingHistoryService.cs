using TradingBot.Backend.Gateway.API.Dtos.Enums;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Repositories;
using TradingBot.Backend.Gateway.API.Responses;

namespace TradingBot.Backend.Gateway.API.Services.Abstract.Api.User
{
	public interface ITradingHistoryService : IServiceRepository<string, TradingHistoryDto, TradingHistoriesDto>
	{
		Task<Response<TradingHistoriesDto>> GetLastOrder(string symbol, string tradingAccountId, OrderType orderType,
			CancellationToken cancellationToken = default);
	}
}
