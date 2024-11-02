using CNG.Abstractions.Signatures;

namespace TradingBot.Backend.Libraries.Domain.Signatures;

public class CreatedBase:ICreated
{
    public string? CreatedUserId { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}