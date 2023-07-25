using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingBot.Backend.Services.Identity.Api.Dtos;
using TradingBot.Backend.Services.Identity.Api.Exceptions;
using TradingBot.Backend.Services.Identity.Api.Extensions;
using TradingBot.Backend.Services.Identity.Api.Middlewares;
using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Services;

public class UserService:IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    #region CRUD

    public async Task<string> InsertAsync(UserInsertDto dto)
    {
        var user = new ApplicationUser()
        {
          
            UserName = dto.UserName,
            Email = dto.Email,
            Gender = dto.Gender,
            Name = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            SurName = dto.SurName,
           BirthDate = dto.BirthDate,
           
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

    public async Task UpdateAsync(UserUpdateDto dto)
    {
        var userCache = await _userManager.FindByIdAsync(dto.Id ?? "") ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

        var rolesCache = await _userManager.GetRolesAsync(userCache) ?? throw new NotFoundException(ErrorDefaults.NotFound.Role);

        var userCacheDto = new UserUpdateDto()
        {
           
            UserName = userCache.UserName,
            Email = userCache.Email,
            Id = userCache.Id,
            Roles = rolesCache.ToList(),
          
            Gender = userCache.Gender,
            Name = userCache.Name,
            PhoneNumber = userCache.PhoneNumber,
            SurName = userCache.SurName,
            UserBirthDate = userCache.BirthDate
        };
        var user = new ApplicationUser()
        {
       
            Email = dto.Email ?? userCache.Email,
            Gender = dto.Gender ?? userCache.Gender,
            Name = dto.Name ?? userCache.Name,
            PhoneNumber = dto.PhoneNumber ?? userCache.PhoneNumber,
            SurName = dto.SurName ?? userCache.SurName,
            UserName = dto.UserName ?? userCache.UserName,
            Id = dto.Id ?? userCache.Id,
            BirthDate = dto.UserBirthDate??userCache.BirthDate
        };
        var response = await _userManager.UpdateAsync(user);

        if (!response.Succeeded)
        {
            await UndoChanges(userCacheDto);
            throw new BadRequestException(string.Join(',', response.Errors.Select(x => x.Description)));
        }

        user = await _userManager.FindByIdAsync(dto.Id ?? "") ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

        if (!string.IsNullOrEmpty(dto.Password))
        {
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            response = await _userManager.ResetPasswordAsync(user, passwordResetToken, dto.Password);

            if (!response.Succeeded)
            {
                await UndoChanges(userCacheDto);
                throw new BadRequestException(string.Join(',', response.Errors.Select(x => x.Description)));

            }

        }

        if (dto.Roles != null) await UpdateUserRoles(userCache.Id, dto.Roles);

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

    #endregion

    #region Roles

    public async Task<List<SelectList<string>>> GetAllRoles()
    {
        var roles =await _roleManager.Roles.ToListAsync();
        return roles.Select(x => new SelectList<string>(x.Id, x.Name ?? "")).ToList();
    }
    public async Task<List<SelectList<string>>> GetUserRolesAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

        var roles = await _userManager.GetRolesAsync(user);
        var allRoles = await _roleManager.Roles.ToListAsync();
        return allRoles.Where(x => roles.Contains(x.Name??"")).Select(x => new SelectList<string>(x.Id, x.Name ?? ""))
            .ToList();
    }

    #endregion

    #region User Confirmations

    public async Task<string> GenerateUserConfirmationToken(string id)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task ValidateUserConfirmationToken(string id,string token)
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

    public async Task PasswordReset(string id,ResetPasswordDto dto)
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

    private async Task UndoChanges(UserUpdateDto userCache)
    {
        var updatedUser = await _userManager.FindByIdAsync(userCache.Id ?? "") ?? throw new NotFoundException(ErrorDefaults.NotFound.User);
        updatedUser.Email = userCache.Email;
        updatedUser.UserName = userCache.UserName;
        updatedUser.Gender = userCache.Gender ?? Gender.Other;
        updatedUser.Name = userCache.Name;
        updatedUser.PhoneNumber = userCache.PhoneNumber;
        updatedUser.SurName = userCache.SurName;
        updatedUser.BirthDate = userCache.UserBirthDate??DateTime.MinValue;

        await _userManager.UpdateAsync(updatedUser);
        if (userCache.Roles != null) await UpdateUserRoles(updatedUser.Id, userCache.Roles);
    }

    private async Task UpdateUserRoles(string userId, List<string> newRoles)
    {
        var user = await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException(ErrorDefaults.NotFound.User);

        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

        await _userManager.AddToRolesAsync(user, newRoles);
    }

    #endregion

}