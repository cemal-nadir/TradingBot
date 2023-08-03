namespace TradingBot.Backend.Gateway.API.Services.Abstract.Gateway
{
	public interface IPlatformGateway
	{
		Task<decimal> GetCurrentBalance(string apiKey, string secretKey, string platform,
			CancellationToken cancellationToken = default);
	}
}
