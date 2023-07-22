
using TradingBot.Backend.Gateway.API.Dtos.Enums;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Users;

public class GetLastOrderDto
{
	public string? Symbol { get; set; }
	public string? TradingAccountId { get; set; }
	public OrderType OrderType { get; set; }
}