using TradingBot.Backend.Gateway.API.Dtos.Enums;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;

public class UsersDto
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public bool IsConfirmed { get; set; }
    public string? Name { get; set; }
    public string? SurName { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthDate { get; set; }
    public string? PhoneNumber { get; set; }
}