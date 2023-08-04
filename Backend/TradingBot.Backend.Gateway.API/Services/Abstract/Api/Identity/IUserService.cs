using CNG.Core;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;
using TradingBot.Backend.Gateway.API.Responses;

namespace TradingBot.Backend.Gateway.API.Services.Abstract.Api.Identity
{
	public interface IUserService
	{
		#region CRUD
		Task<Response<List<UsersDto>>> GetAllUsers(CancellationToken cancellationToken = default);

		Task<Response<List<UsersDto>>> GetAllByNameSurname(string? searchText,
			CancellationToken cancellationToken = default);
		Task<Response<List<UsersDto>>> GetAllByUserIdsAsync(List<string> listOfId,
			CancellationToken cancellationToken = default);
		Task<Response<UserDto>> GetUser(string id, CancellationToken cancellationToken = default);
		Task<Response> InsertUser(UserDto dto, CancellationToken cancellationToken = default);
		Task<Response> UpdateUser(string id,UserDto dto, CancellationToken cancellationToken = default);
		Task<Response> DeleteUser(string id, CancellationToken cancellationToken = default);
		Task<Response<UserDto>> GetCurrentUser(CancellationToken cancellationToken = default);
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
