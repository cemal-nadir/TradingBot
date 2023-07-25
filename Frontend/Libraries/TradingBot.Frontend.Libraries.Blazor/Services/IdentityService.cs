using System.Security.Authentication;
using System.Security.Claims;
using CNG.Core.Exceptions;
using CNG.Http.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Helpers;
using TradingBot.Frontend.Libraries.Blazor.Models;

namespace TradingBot.Frontend.Libraries.Blazor.Services
{
	public class IdentityService : ServerAuthenticationStateProvider, IIdentityService
	{
		private readonly IIdentityServerService _identityServerService;
		private readonly IHttpClientService _httpClientService;
		private readonly ProtectedLocalStorage _localStorageService;
		public IdentityService(Env env, IIdentityServerService identityServerService, ProtectedLocalStorage localStorageService, IHttpClientService httpClientService)
		{
			_identityServerService = identityServerService;
			_localStorageService = localStorageService;
			_httpClientService = httpClientService;
			_identityServerService.SetClient(env.IdentityUrl, env.Client.Id ?? "", env.Client.Secret ?? "", ClientDefaults.DefaultClient);
		}
		public async Task GetAccessTokenByRefreshToken(CancellationToken cancellationToken = default)
		{
			var authenticationToken = (await _localStorageService.GetAsync<AuthenticationToken>(nameof(AuthenticationToken))).Value;
			var tokenResponse = await _identityServerService.GetAccessTokenByRefreshToken(authenticationToken?.RefreshToken ?? "", false, cancellationToken);
			if (!tokenResponse.Success) throw new AuthenticationException(tokenResponse.Message);

			authenticationToken = new AuthenticationToken()
			{
				AccessToken = tokenResponse.Data?.AccessToken,
				RefreshToken = tokenResponse.Data?.RefreshToken,
				Expired = DateTime.Now.AddSeconds(tokenResponse.Data?.ExpiresIn ?? 0)
			};

			await _localStorageService.SetAsync(nameof(AuthenticationToken),
				authenticationToken);

			NotifyAuthenticationStateChanged(Task.FromResult(
				new AuthenticationState(
					new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(authenticationToken.AccessToken ?? ""))))));

			if (tokenResponse.Data is null) throw new NotFoundException(ErrorDefaults.NotFound.RemoteApiEmptyResponse);
		}

		public async Task SignOut(CancellationToken cancellationToken = default)
		{

			var authenticationToken = (await _localStorageService.GetAsync<AuthenticationToken>(nameof(AuthenticationToken))).Value;

			await _identityServerService.RevokeRefreshToken(authenticationToken?.RefreshToken ?? "", false, cancellationToken);

			await _localStorageService.DeleteAsync(nameof(AuthenticationToken));

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
		}

		public async Task SignIn(string username, string? password, bool rememberMe, CancellationToken cancellationToken = default)
		{
			var userResponse = await _identityServerService.SignIn(
				username, password??"", false, cancellationToken);

			if (!userResponse.Success)
				throw new BadRequestException(userResponse.Message);


			if (userResponse.Data?.UserInfo is null)
				throw new NotFoundException(ErrorDefaults.NotFound.UserInfoNotFound);

			if (userResponse.Data?.Token is null)
				throw new NotFoundException(ErrorDefaults.NotFound.TokenNotFound);

			var authenticationToken = new AuthenticationToken()
			{
				AccessToken = userResponse.Data?.Token.AccessToken,
				RefreshToken = userResponse.Data?.Token.RefreshToken,
				Expired = DateTime.Now.AddSeconds(userResponse.Data?.Token.ExpiresIn ?? 0)
			};

			await _localStorageService.SetAsync(nameof(AuthenticationToken),
				authenticationToken);

			NotifyAuthenticationStateChanged(Task.FromResult(
				new AuthenticationState(
					new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(authenticationToken.AccessToken ?? ""))))));
		}
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{

			var token = (await _localStorageService.GetAsync<AuthenticationToken>(nameof(AuthenticationToken))).Value;

			if (token?.AccessToken == null || string.IsNullOrEmpty(token.AccessToken))
			{
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			if (token.Expired >= DateTime.Now)
				return new AuthenticationState(
					new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token.AccessToken))));
			await GetAccessTokenByRefreshToken();
			token = (await _localStorageService.GetAsync<AuthenticationToken>(nameof(AuthenticationToken))).Value;
			_httpClientService.SetBearerAuthentication(token?.AccessToken??"");
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token?.AccessToken ?? ""))));

		}
	}
}
