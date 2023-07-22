using CNG.MongoDB;
using CNG.MongoDB.Context;
using MongoDB.Driver;
using TradingBot.Backend.Libraries.Application.Repositories.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;
using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Libraries.Persistence.Repositories.User
{
	public class TradingHistoryRepository:MongoDbRepository<TradingHistory,string>,ITradingHistoryRepository
	{
		public TradingHistoryRepository(IMongoDbContext context) : base(context)
		{
		}
		public async Task<TradingHistory?> GetLastOrderForSymbolAsync(string symbol,string tradingAccountId,OrderType orderType,
			CancellationToken cancellationToken = default)
		{
			var builder = Builders<TradingHistory>.Filter;
			var collection = await GetCollectionAsync(cancellationToken);
			var filter = builder.Eq(x => x.Symbol, symbol);
			filter &= builder.Eq(x => x.IsClosed, false);
			filter &= builder.Eq(x => x.TradingAccountId, tradingAccountId);
			filter &= builder.Eq(x => x.OrderType, orderType);
			var sort = Builders<TradingHistory>.Sort.Descending(x => x.CreatedAt);
			var model = await collection.Find(filter).Sort(sort)
				.FirstOrDefaultAsync(cancellationToken);

			return model;
		}
	}
}
