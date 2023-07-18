namespace TradingBot.Backend.Gateway.API.Services.Abstract.Token
{
	public interface ITokenService
	{
		Task<string> GetToken(CancellationToken cancellationToken=default);
	}
}
