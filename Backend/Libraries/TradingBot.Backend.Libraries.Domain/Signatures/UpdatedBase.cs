using CNG.Abstractions.Signatures;

namespace TradingBot.Backend.Libraries.Domain.Signatures;

public class UpdatedBase:CreatedBase,IUpdated
{
    public string? UpdatedUserId { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}