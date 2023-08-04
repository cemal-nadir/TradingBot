using CNG.Core;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Extensions;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Identity;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Gateway
{
	public class UserGateway : IUserGateway
	{
		private readonly ITradingAccountService _tradingAccountService;
		private readonly IUserService _userService;

		public UserGateway(ITradingAccountService tradingAccountService, IUserService userService)
		{
			_tradingAccountService = tradingAccountService;
			_userService = userService;
		}

		#region Identity

		#region CRUD


		public async Task<List<UsersDto>?> GetAllUsers(CancellationToken cancellationToken = default)
		{
			return (await _userService.GetAllUsers(cancellationToken)).CheckResponse();

		}

		public async Task<List<UsersDto>?> GetAllByNameSurname(string? searchText, CancellationToken cancellationToken = default)
		{
			return (await _userService.GetAllByNameSurname(searchText,cancellationToken)).CheckResponse();
		}


		public async Task<UserDto> GetUser(string id, CancellationToken cancellationToken = default)
		{
			return (await _userService.GetUser(id, cancellationToken)).CheckResponse();

		}


		public async Task InsertUser(UserDto dto, CancellationToken cancellationToken = default)
		{
			(await _userService.InsertUser(dto, cancellationToken)).CheckResponse();

		}


		public async Task UpdateUser(string id,UserDto dto, CancellationToken cancellationToken = default)
		{
			(await _userService.UpdateUser(id,dto, cancellationToken)).CheckResponse();
		}


		public async Task DeleteUser(string id, CancellationToken cancellationToken = default)
		{
			(await _userService.DeleteUser(id, cancellationToken)).CheckResponse();

		}


		public async Task<UserDto> GetCurrentUser(CancellationToken cancellationToken = default)
		{
			return (await _userService.GetCurrentUser(cancellationToken)).CheckResponse();
		}

		#endregion

		#region Roles


		public async Task<List<SelectList<string>>?> GetRoles(CancellationToken cancellationToken = default)
		{
			return (await _userService.GetRoles(cancellationToken)).CheckResponse();

		}


		public async Task<List<SelectList<string>>?> GetUserRoles(string id, CancellationToken cancellationToken = default)
		{
			return (await _userService.GetUserRoles(id, cancellationToken)).CheckResponse();

		}


		#endregion

		#region User Confirmation


		public async Task<string> GenerateUserConfirmToken(string id, CancellationToken cancellationToken = default)
		{
			return (await _userService.GenerateUserConfirmToken(id, cancellationToken)).CheckResponse();
		}


		public async Task ConfirmUserToken(string id, string token, CancellationToken cancellationToken = default)
		{
			(await _userService.ConfirmUserToken(id, token, cancellationToken)).CheckResponse();
		}

		#endregion

		#region Password Reset


		public async Task<string> GeneratePassWordResetToken(string id, CancellationToken cancellationToken = default)
		{
			return (await _userService.GeneratePassWordResetToken(id, cancellationToken)).CheckResponse();
		}

		public async Task ResetPassword(string id, ResetPasswordDto dto, CancellationToken cancellationToken = default)
		{
			(await _userService.ResetPassword(id,dto, cancellationToken)).CheckResponse();
		}


		public async Task ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken = default)
		{
			(await _userService.ChangePassword(dto, cancellationToken)).CheckResponse();

		}

		#endregion


		#endregion


		#region Trading Accounts

		public async Task<TradingAccountDto> GetTradingAccountAsync(string tradingAccountId,
			CancellationToken cancellationToken = default) =>
			(await _tradingAccountService.GetAsync(tradingAccountId, cancellationToken)).CheckResponse();

		public async Task<List<TradingAccountsDto>?> GetTradingAccountsAsync(
			CancellationToken cancellationToken = default)
		{
			var tradingAccounts=(await _tradingAccountService.GetAllAsync(cancellationToken)).CheckResponse();
			var users = (await _userService.GetAllByUserIdsAsync(tradingAccounts?.Select(x=>x.UserId).Distinct().ToList()!  , cancellationToken)).CheckResponse();
			return tradingAccounts?.Select(x => new TradingAccountsDto
			{
				BalanceSettings = x.BalanceSettings,
				UserId = x.UserId,
				IsActive = x.IsActive,
				ApiKey = x.ApiKey,
				Id = x.Id,
				Name = x.Name,
				SecretKey = x.SecretKey,
				Indicators = x.Indicators,
				Platform = x.Platform,
				UserName =
					$"{users?.FirstOrDefault(y => y.Id == x.UserId)?.Name} {users?.FirstOrDefault(y => y.Id == x.UserId)?.SurName}",
				UserMail = users?.FirstOrDefault(y => y.Id == x.UserId)?.Email
			}).ToList();
		}

		public async Task<List<TradingAccountsDto>?> GetTradingAccountsByUserIdAsync(string userId,
			CancellationToken cancellationToken = default)
		{
			var tradingAccounts=(await _tradingAccountService.GetAllByUserIdAsync(userId, cancellationToken)).CheckResponse();
			var user = (await _userService.GetUser(userId, cancellationToken)).CheckResponse();
			return tradingAccounts?.Select(x => new TradingAccountsDto
			{
				BalanceSettings = x.BalanceSettings,
				UserId = x.UserId,
				IsActive = x.IsActive,
				ApiKey = x.ApiKey,
				Id = x.Id,
				Name = x.Name,
				SecretKey = x.SecretKey,
				Indicators = x.Indicators,
				Platform = x.Platform,
				UserName =
					$"{user.Name} {user.SurName}",
				UserMail = user.Email
			}).ToList();
		}

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
