namespace TradingBot.Frontend.Libraries.Blazor.Services
{
	public interface IIdentityService
	{
		Task GetAccessTokenByRefreshToken(CancellationToken cancellationToken = default);
		Task SignOut(CancellationToken cancellationToken = default);
		Task SignIn(string username, string password, bool rememberMe, CancellationToken cancellationToken = default);
	}
}
