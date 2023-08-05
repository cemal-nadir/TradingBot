using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingBot.Backend.Services.Identity.Api.Dtos;
using TradingBot.Backend.Services.Identity.Api.Exceptions;
using TradingBot.Backend.Services.Identity.Api.Extensions;
using TradingBot.Backend.Services.Identity.Api.Middlewares;
using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Services;

public class UserService : IUserService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	#region CRUD

	public async Task<string> InsertAsync(UserDto dto)
	{
		var user = new ApplicationUser()
		{

			UserName = dto.UserName,
			Email = dto.Email,
			Gender = dto.Gender,
			Name = dto.Name,
			PhoneNumber = dto.PhoneNumber,
			SurName = dto.SurName,
			BirthDate =DateTime.SpecifyKind(dto.BirthDate,DateTimeKind.Utc),
			EmailConfirmed = dto.IsConfirmed,
			IsBanned = false,
			

		};
		var result = await _userManager.CreateAsync(user, dto.Password ?? "");
		if (!result.Succeeded)
			throw new BadRequestException(string.Join(',', result.Errors.Select(x => x.Description)));
		if (dto.Roles == null) return user.Id;

		foreach (var role in dto.Roles)
		{
			var newRole = await _roleManager.FindByNameAsync(role);
			if (newRole is null)
			{
				await DeleteUser(user.Id);
				throw new NotFoundException(ErrorDefaults.NotFound.Role);
			}

			result = await _userManager.AddToRoleAsync(user, newRole.Name ?? "");
			if (result.Succeeded) continue;
			await DeleteUser(user.Id);

			throw new BadRequestException(string.Join(',', result.Errors.Select(x => x.Description)));
		}

		return user.Id;
	}

	public async Task UpdateAsync(string id, UserDto dto)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);
		
		var rolesCache = await _userManager.GetRolesAsync(user) ?? throw new NotFoundException(ErrorDefaults.NotFound.Role);

		var userCacheDto = new UserDto()
		{

			UserName = user.UserName,
			Email = user.Email,
			Roles = rolesCache.ToList(),
			Gender = user.Gender,
			Name = user.Name,
			PhoneNumber = user.PhoneNumber,
			SurName = user.SurName,
			BirthDate = user.BirthDate,
			IsConfirmed = user.EmailConfirmed
		};

		user.Email=dto.Email;
		user.Gender = dto.Gender;
		user.Name = dto.Name;
		user.PhoneNumber=dto.PhoneNumber;
		user.SurName=dto.SurName;
		user.UserName = dto.UserName;
		user.BirthDate=DateTime.SpecifyKind(dto.BirthDate,DateTimeKind.Utc);
		user.EmailConfirmed = dto.IsConfirmed;

		var response = await _userManager.UpdateAsync(user);

		if (!response.Succeeded)
		{
			await UndoChanges(id,userCacheDto);
			throw new BadRequestException(string.Join(',', response.Errors.Select(x => x.Description)));
		}

		user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

		if (!string.IsNullOrEmpty(dto.Password))
		{
			var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
			response = await _userManager.ResetPasswordAsync(user, passwordResetToken, dto.Password);

			if (!response.Succeeded)
			{
				await UndoChanges(id,userCacheDto);
				throw new BadRequestException(string.Join(',', response.Errors.Select(x => x.Description)));
			}

		}

		if (dto.Roles != null) await UpdateUserRoles(user.Id, dto.Roles);

	}

	public async Task DeleteAsync(string id)
	{
		await DeleteUser(id);
	}

	public async Task<UserDto> GetAsync(string id)
	{
		return await GetUserById(id);
	}

	public async Task<List<UsersDto>> GetAllAsync()
	{
		var models = await _userManager.Users.ToListAsync();
		return models.Select(x => x.MapUsers()).ToList();
	}

	public async Task<List<UsersDto>> GetAllByNameSurname(string? searchText,
		CancellationToken cancellationToken = default)
	{
		if (searchText is null)
			return (await _userManager.Users.ToListAsync(cancellationToken)).Select(x => x.MapUsers()).ToList();
		searchText = searchText.ToLower();
		var split = searchText.Split(' ').Where(x => x.Length > 1).ToList();
		if (split.Count == 1)
		{
			return (await _userManager.Users
				.Where(x => x.SurName != null && x.Name != null &&
							(x.Name.ToLower().Contains(split[0]) || x.SurName.ToLower().Contains(split[0]))).ToListAsync(cancellationToken)).Select(x => x.MapUsers()).ToList();
		}


		return (await _userManager.Users
			.Where(x => x.SurName != null && x.Name != null &&
						(x.Name.ToLower().Contains(string.Join(' ', split.Take(split.Count - 1))) ||
						 x.SurName.ToLower().Contains(split.Last()) ||
						 (x.Name.ToLower() + " " + x.SurName.ToLower())
						 .Contains($"{string.Join(' ', split.Take(split.Count - 1))} {split.Last()}"))).ToListAsync(cancellationToken)).Select(x => x.MapUsers()).ToList();




	}
	public async Task<List<UsersDto>> GetUsersByIdsAsync(List<string> listOfId, CancellationToken cancellationToken = default)
	{
		var models = await _userManager.Users.Where(x => listOfId.Contains(x.Id)).ToListAsync(cancellationToken);
		return models.Select(x => x.MapUsers()).ToList();
	}
	#endregion

	#region Roles

	public async Task<List<SelectList<string>>> GetAllRoles()
	{
		var roles = await _roleManager.Roles.ToListAsync();
		return roles.Select(x => new SelectList<string>(x.Id, x.Name ?? "")).ToList();
	}
	public async Task<List<SelectList<string>>> GetUserRolesAsync(string id)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

		var roles = await _userManager.GetRolesAsync(user);
		var allRoles = await _roleManager.Roles.ToListAsync();
		return allRoles.Where(x => roles.Contains(x.Name ?? "")).Select(x => new SelectList<string>(x.Id, x.Name ?? ""))
			.ToList();
	}

	#endregion

	#region User Confirmations

	public async Task<string> GenerateUserConfirmationToken(string id)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

		return await _userManager.GenerateEmailConfirmationTokenAsync(user);
	}

	public async Task ValidateUserConfirmationToken(string id, string token)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);
		var result = await _userManager.ConfirmEmailAsync(user, token);

		if (!result.Succeeded)
			throw new BadRequestException(string.Join(',', result.Errors.Select(x => x.Description)));
	}
	#endregion

	#region Password Reset

	public async Task<string> GeneratePasswordResetToken(string id)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

		return await _userManager.GeneratePasswordResetTokenAsync(user);
	}

	public async Task PasswordReset(string id, ResetPasswordDto dto)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

		var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
		if (!result.Succeeded)
		{
			throw new BadRequestException(string.Join(',', result.Errors.Select(x => x.Description)));
		}
	}

	public async Task ChangePassword(string id, ChangePasswordDto dto)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

		var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

		if (!result.Succeeded)
		{
			throw new BadRequestException(string.Join(',', result.Errors.Select(x => x.Description)));
		}
	}
	#endregion

	#region Service Helpers

	private async Task<UserDto> GetUserById(string id)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);
		var roles = await _userManager.GetRolesAsync(user);

		return user.MapUser(roles.ToList());
	}
	private async Task DeleteUser(string id)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);
		await _userManager.DeleteAsync(user);
	}

	private async Task UndoChanges(string id,UserDto userCache)
	{
		var updatedUser = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);
		updatedUser.Email = userCache.Email;
		updatedUser.UserName = userCache.UserName;
		updatedUser.Gender = userCache.Gender;
		updatedUser.Name = userCache.Name;
		updatedUser.PhoneNumber = userCache.PhoneNumber;
		updatedUser.SurName = userCache.SurName;
		updatedUser.BirthDate = userCache.BirthDate;
		await _userManager.UpdateAsync(updatedUser);
		if (userCache.Roles != null) await UpdateUserRoles(updatedUser.Id, userCache.Roles);
	}

	private async Task UpdateUserRoles(string id, List<string> newRoles)
	{
		var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

		await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

		await _userManager.AddToRolesAsync(user, newRoles);
	}

	#endregion

}