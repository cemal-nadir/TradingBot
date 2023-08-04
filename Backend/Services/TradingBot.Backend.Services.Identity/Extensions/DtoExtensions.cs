using TradingBot.Backend.Services.Identity.Api.Dtos;
using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Extensions;

public static class DtoExtensions
{
    public static UserDto MapUser(this ApplicationUser user,List<string>roles)
    {
        return new UserDto
        {
          
            Email = user.Email,
            Gender = user.Gender,
            IsConfirmed = user.EmailConfirmed,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            SurName = user.SurName,
            UserName = user.UserName,
            Roles = roles,
            BirthDate = user.BirthDate
        };
    }
    public static UsersDto MapUsers(this ApplicationUser user)
    {
        return new UsersDto
        {
            Email = user.Email,
            Gender = user.Gender,
            Id = user.Id,
            IsConfirmed = user.EmailConfirmed,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber,
            SurName = user.SurName,
            UserName = user.UserName,
            BirthDate = user.BirthDate
        };
    }
}