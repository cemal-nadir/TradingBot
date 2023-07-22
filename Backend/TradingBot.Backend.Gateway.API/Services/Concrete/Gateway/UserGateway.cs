using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Extensions;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Gateway
{
	public class UserGateway:IUserGateway
	{
		private readonly ITradingAccountService _tradingAccountService;

		public UserGateway(ITradingAccountService tradingAccountService)
		{
			_tradingAccountService = tradingAccountService;
		}

		#region Trading Accounts

		public async Task<TradingAccountDto> GetTradingAccountAsync(string tradingAccountId,
			CancellationToken cancellationToken = default) =>
			(await _tradingAccountService.GetAsync(tradingAccountId, cancellationToken)).CheckResponse();

		public async Task<List<TradingAccountsDto>?> GetTradingAccountsAsync(
			CancellationToken cancellationToken = default) =>
			(await _tradingAccountService.GetAllAsync(cancellationToken)).CheckResponse();

		public async Task InsertTradingAccountAsync(TradingAccountDto dto,
			CancellationToken cancellationToken = default) =>
			(await _tradingAccountService.InsertAsync(dto, cancellationToken)).CheckResponse();
		public async Task UpdateTradingAccountAsync(string tradingAccountId, TradingAccountDto dto,
			CancellationToken cancellationToken = default) =>
			(await _tradingAccountService.UpdateAsync(tradingAccountId, dto, cancellationToken)).CheckResponse();
		public async Task DeleteTradingAccountAsync(string tradingAccountId,
			CancellationToken cancellationToken = default) =>
			(await _tradingAccountService.DeleteAsync(tradingAccountId, cancellationToken)).CheckResponse();

		public async Task DeleteRangeTradingAccountsAsync(IEnumerable<string> tradingAccountIds,
			CancellationToken cancellationToken = default) =>
			(await _tradingAccountService.DeleteRangeAsync(tradingAccountIds, cancellationToken)).CheckResponse();

		#endregion

		#region Indicators

		

		#endregion

	}
}
