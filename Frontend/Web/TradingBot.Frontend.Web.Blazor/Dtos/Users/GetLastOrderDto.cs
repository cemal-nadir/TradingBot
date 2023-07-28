
using TradingBot.Frontend.Web.Blazor.Dtos.Enums;

namespace TradingBot.Frontend.Web.Blazor.Dtos.Users;

public class GetLastOrderDto
{
	public string? Symbol { get; set; }
	public string? TradingAccountId { get; set; }
	public OrderType OrderType { get; set; }
}