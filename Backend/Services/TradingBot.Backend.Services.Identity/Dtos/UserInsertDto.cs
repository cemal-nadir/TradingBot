using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Dtos;

public class UserInsertDto
{
    public string? Name { get; set; }
    public string? SurName { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public List<string>? Roles { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
}