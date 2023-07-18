using CNG.Abstractions.Repositories;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Libraries.Application.Repositories.User
{
	public interface ITradingHistoryRepository: IMongoDbRepository<TradingHistory,string>
	{
	}
}
