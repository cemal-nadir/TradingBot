﻿using CNG.Core;
using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Web.Blazor.Dtos.Identity;

namespace TradingBot.Frontend.Web.Blazor.Services.Abstract.User
{
	public interface IUserService
	{
		#region CRUD

		Task<Response<List<UsersDto>>> GetAllUsers(CancellationToken cancellationToken = default);
		Task<Response<UserDto>> GetUser(string id, CancellationToken cancellationToken = default);
		Task<Response> InsertUser(UserInsertDto dto, CancellationToken cancellationToken = default);
		Task<Response> UpdateUser(UserUpdateDto dto, CancellationToken cancellationToken = default);
		Task<Response> DeleteUser(string id, CancellationToken cancellationToken = default);
		Task<Response<UserDto>> GetCurrentUser(CancellationToken cancellationToken = default);

		Task<Response<List<UsersDto>>> GetAllByNameSurname(string? searchText,
			CancellationToken cancellationToken = default);

		#endregion

		#region Roles

		Task<Response<List<SelectList<string>>>> GetRoles(CancellationToken cancellationToken = default);
		Task<Response<List<SelectList<string>>>> GetUserRoles(string id, CancellationToken cancellationToken = default);

		#endregion

		#region User Confirmation

		Task<Response<string>> GenerateUserConfirmToken(string id, CancellationToken cancellationToken = default);
		Task<Response> ConfirmUserToken(string id, string token, CancellationToken cancellationToken = default);

		#endregion

		#region Password Reset

		Task<Response<string>> GeneratePassWordResetToken(string id, CancellationToken cancellationToken = default);
		Task<Response> ResetPassword(string id, ResetPasswordDto dto, CancellationToken cancellationToken = default);
		Task<Response> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken = default);

		#endregion
	}
}
