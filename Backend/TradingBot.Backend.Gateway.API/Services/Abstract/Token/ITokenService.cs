namespace TradingBot.Backend.Gateway.API.Services.Abstract.Token
{
	public interface ITokenService
	{
		Task<string> GetClientCredentialToken(CancellationToken cancellationToken=default);
		string GetResourceOwnerPasswordToken(CancellationToken cancellationToken = default);
	}
}
