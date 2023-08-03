using TradingBot.Backend.Services.Identity.Api.Dtos;

namespace TradingBot.Backend.Services.Identity.Api.Services;

public interface IUserService
{
    #region CRUD

    Task<string> InsertAsync(UserInsertDto dto);
    Task UpdateAsync(UserUpdateDto dto);
    Task DeleteAsync(string id);
    Task<UserDto> GetAsync(string id);
    Task<List<UsersDto>> GetAllAsync();
    Task<List<UsersDto>> GetUsersByIdsAsync(List<string> listOfId, CancellationToken cancellationToken = default);

    Task<List<UsersDto>> GetAllByNameSurname(string searchText,
	    CancellationToken cancellationToken = default);
	#endregion

	#region Roles

	Task<List<SelectList<string>>> GetUserRolesAsync(string id);
    Task<List<SelectList<string>>> GetAllRoles();

    #endregion

    #region User Confirmations

    Task<string> GenerateUserConfirmationToken(string id);
    Task ValidateUserConfirmationToken(string id, string token);

    #endregion

    #region Password Reset

    Task<string> GeneratePasswordResetToken(string id);
    Task PasswordReset(string id, ResetPasswordDto dto);
    Task ChangePassword(string id, ChangePasswordDto dto);

    #endregion

}