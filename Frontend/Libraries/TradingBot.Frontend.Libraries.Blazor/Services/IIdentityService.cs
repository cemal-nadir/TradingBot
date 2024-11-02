using Microsoft.AspNetCore.Components.Authorization;
using TradingBot.Frontend.Libraries.Blazor.Responses;

namespace TradingBot.Frontend.Libraries.Blazor.Services
{
	public interface IIdentityService
	{
		Task GetAccessTokenByRefreshToken(CancellationToken cancellationToken = default);
		Task<Response> SignOut(CancellationToken cancellationToken = default);
		Task<Response> SignIn(string username, string password, CancellationToken cancellationToken = default);
        Task<AuthenticationState> GetAuthenticationStateAsync();

    }
}
