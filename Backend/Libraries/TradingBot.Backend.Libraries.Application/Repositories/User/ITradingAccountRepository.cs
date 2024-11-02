using CNG.Abstractions.Repositories;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Libraries.Application.Repositories.User
{
    public interface ITradingAccountRepository : IMongoDbRepository<TradingAccount, string>
    {
	    Task<TradingAccount?> GetByIndicatorId(string indicatorId,
		    CancellationToken cancellationToken = default);

        Task UpdateAdjustedBalances(
            CancellationToken cancellationToken = default);

    }
}
