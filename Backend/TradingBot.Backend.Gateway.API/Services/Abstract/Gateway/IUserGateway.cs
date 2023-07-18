using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;

namespace TradingBot.Backend.Gateway.API.Services.Abstract.Gateway
{
	public interface IUserGateway
	{
		Task<TradingAccountDto> GetTradingAccountAsync(string tradingAccountId,
			CancellationToken cancellationToken = default);

		Task<List<TradingAccountsDto>?> GetTradingAccountsAsync(
			CancellationToken cancellationToken = default);
		Task InsertTradingAccountAsync(TradingAccountDto dto,
			CancellationToken cancellationToken = default);

		Task UpdateTradingAccountAsync(string tradingAccountId, TradingAccountDto dto,
			CancellationToken cancellationToken = default);

		Task DeleteTradingAccountAsync(string tradingAccountId,
			CancellationToken cancellationToken = default);

		Task DeleteRangeTradingAccountsAsync(IEnumerable<string> tradingAccountIds,
			CancellationToken cancellationToken = default);
	}
}
