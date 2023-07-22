using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Libraries.Application.Dtos.User;

public class GetLastOrderDto
{
	public string? Symbol { get; set; }
	public string? TradingAccountId { get; set; }
	public OrderType OrderType { get; set; }
}