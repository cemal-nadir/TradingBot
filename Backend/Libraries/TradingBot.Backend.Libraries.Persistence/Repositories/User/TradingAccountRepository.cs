using CNG.MongoDB;
using CNG.MongoDB.Context;
using MongoDB.Driver;
using TradingBot.Backend.Libraries.Application.Repositories.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Libraries.Persistence.Repositories.User
{
	public class TradingAccountRepository:MongoDbRepository<TradingAccount,string>,ITradingAccountRepository
	{
		public TradingAccountRepository(IMongoDbContext context) : base(context)
		{
		}

		public async Task<TradingAccount?> GetByIndicatorId(string indicatorId,
			CancellationToken cancellationToken = default)
		{
			var builder = Builders<TradingAccount>.Filter;
			var collection = await GetCollectionAsync(cancellationToken);
			var model = await collection.Find(builder.Regex($"{nameof(TradingAccount.Indicators)}.{nameof(Indicator.Id)}", indicatorId))
				.FirstOrDefaultAsync(cancellationToken);

			return model;
		}

	}
}
