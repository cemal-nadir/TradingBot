using CNG.MongoDB;
using CNG.MongoDB.Context;
using TradingBot.Backend.Libraries.Application.Repositories.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Libraries.Persistence.Repositories.User
{
	public class TradingHistoryRepository:MongoDbRepository<TradingHistory,string>,ITradingHistoryRepository
	{
		public TradingHistoryRepository(IMongoDbContext context) : base(context)
		{
		}
	}
}
