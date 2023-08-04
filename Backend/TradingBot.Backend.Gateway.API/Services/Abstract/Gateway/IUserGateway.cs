using CNG.Core;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;

namespace TradingBot.Backend.Gateway.API.Services.Abstract.Gateway
{
	public interface IUserGateway
	{
		#region Identity

		#region CRUD

		Task<List<UsersDto>?> GetAllUsers(CancellationToken cancellationToken = default);
		Task<List<UsersDto>?> GetAllByNameSurname(string? searchText,CancellationToken cancellationToken = default);
		Task<UserDto> GetUser(string id, CancellationToken cancellationToken = default);
		Task InsertUser(UserDto dto, CancellationToken cancellationToken = default);
		Task UpdateUser(string id,UserDto dto, CancellationToken cancellationToken = default);
		Task DeleteUser(string id, CancellationToken cancellationToken = default);
		Task<UserDto> GetCurrentUser(CancellationToken cancellationToken = default);
		#endregion

		#region Roles

		Task<List<SelectList<string>>?> GetRoles(CancellationToken cancellationToken = default);
		Task<List<SelectList<string>>?> GetUserRoles(string id, CancellationToken cancellationToken = default);

		#endregion

		#region User Confirmation

		Task<string> GenerateUserConfirmToken(string id, CancellationToken cancellationToken = default);
		Task ConfirmUserToken(string id, string token, CancellationToken cancellationToken = default);

		#endregion

		#region Password Reset

		Task<string> GeneratePassWordResetToken(string id, CancellationToken cancellationToken = default);
		Task ResetPassword(string id, ResetPasswordDto dto, CancellationToken cancellationToken = default);
		Task ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken = default);

		#endregion

		#endregion

		#region Trading Accounts

		Task<TradingAccountDto> GetTradingAccountAsync(string tradingAccountId,
			CancellationToken cancellationToken = default);

		Task<List<TradingAccountsDto>?> GetTradingAccountsAsync(
			CancellationToken cancellationToken = default);

		Task<List<TradingAccountsDto>?> GetTradingAccountsByUserIdAsync(string userId,
			CancellationToken cancellationToken = default);
		Task InsertTradingAccountAsync(TradingAccountDto dto,
			CancellationToken cancellationToken = default);

		Task UpdateTradingAccountAsync(string tradingAccountId, TradingAccountDto dto,
			CancellationToken cancellationToken = default);

		Task DeleteTradingAccountAsync(string tradingAccountId,
			CancellationToken cancellationToken = default);

		Task DeleteRangeTradingAccountsAsync(IEnumerable<string> tradingAccountIds,
			CancellationToken cancellationToken = default);

		#endregion

	}
}
