using Microsoft.AspNetCore.Identity;

namespace TradingBot.Backend.Services.Identity.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }
    public string? SurName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public bool IsBanned { get; set; }
}
public enum Gender
{
    Male,
    Female,
    Other
}