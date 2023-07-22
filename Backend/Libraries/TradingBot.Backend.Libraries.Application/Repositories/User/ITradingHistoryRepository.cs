using CNG.Abstractions.Repositories;
using TradingBot.Backend.Libraries.Domain.Entities.User;
using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Libraries.Application.Repositories.User
{
	public interface ITradingHistoryRepository: IMongoDbRepository<TradingHistory,string>
	{
		Task<TradingHistory?> GetLastOrderForSymbol(string symbol, string tradingAccountId, OrderType orderType,
			CancellationToken cancellationToken = default);
	}
}
