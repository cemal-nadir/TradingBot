using CNG.MongoDB;
using CNG.MongoDB.Context;
using MongoDB.Bson;
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
			var model = await collection.Find(builder.Regex($"{nameof(TradingAccount.Indicators)}.{nameof(TradingAccount.Indicator.Id)}", indicatorId))
				.FirstOrDefaultAsync(cancellationToken);

			return model;
		}
        public async Task UpdateAdjustedBalances(
            CancellationToken cancellationToken = default)
        {
            var builder = Builders<TradingAccount>.Filter;
            var collection = await GetCollectionAsync(cancellationToken);
            var projectDefinition = Builders<TradingAccount>.Projection.Expression(x => new
            {
                x.Id, x.BalanceSettings!.Plan!.AdjustFrequencyDay, x.BalanceSettings.Plan.LastAdjust
            });
            
            var filter = Builders<TradingAccount>.Filter.Empty;
            var models = await collection.Find(filter).Project(projectDefinition)
                .ToListAsync(cancellationToken);
            var now=  DateTime.UtcNow;
            var setDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
            var listOfIds = models
                .Where(x => x.LastAdjust != null && x.LastAdjust.Value.AddDays(x.AdjustFrequencyDay) <= setDate)
                .Select(x => x.Id).ToList();

            if (!listOfIds.Any()) return;

            var dataList = await collection.Find(builder.In(x => x.Id, listOfIds)).ToListAsync(cancellationToken);

            dataList.ForEach(data =>
            {
                data.BalanceSettings!.Plan!.CurrentAdjustedBalance = (data.BalanceSettings.CurrentBalance *
                                                                    data.BalanceSettings.Plan.AdjustBalancePercentage) /
                                                                   100;
                data.BalanceSettings.Plan.LastAdjust = setDate;
            });

            await UpdateRangeAsync(dataList, cancellationToken);

        }

    }
}
