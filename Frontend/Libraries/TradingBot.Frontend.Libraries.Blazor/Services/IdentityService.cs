using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using CNG.Core.Exceptions;
using CNG.Http.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Helpers;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Responses;

namespace TradingBot.Frontend.Libraries.Blazor.Services
{
	public class IdentityService : AuthenticationStateProvider, IIdentityService
	{
		private readonly IIdentityServerService _identityServerService;
		private readonly ProtectedLocalStorage _localStorageService;
		public IdentityService(Env env, IIdentityServerService identityServerService, ProtectedLocalStorage localStorageService)
		{
			_identityServerService = identityServerService;
			_localStorageService = localStorageService;
			_identityServerService.SetClient(env.IdentityUrl, env.Client.Id ?? "", env.Client.Secret ?? "", ClientDefaults.DefaultClient);
		}
		public async Task GetAccessTokenByRefreshToken(CancellationToken cancellationToken = default)
		{
			var authenticationToken = (await _localStorageService.GetAsync<AuthenticationTokenWithClaims>(nameof(AuthenticationTokenWithClaims))).Value;
			var tokenResponse = await _identityServerService.GetAccessTokenByRefreshToken(authenticationToken?.RefreshToken ?? "", false, cancellationToken);
			if (!tokenResponse.Success) throw new AuthenticationException(tokenResponse.Message);

			authenticationToken = new AuthenticationTokenWithClaims()
			{
				AccessToken = tokenResponse.Data?.AccessToken,
				RefreshToken = tokenResponse.Data?.RefreshToken,
				Expired = DateTime.Now.AddSeconds(tokenResponse.Data?.ExpiresIn ?? 0),
				Claims = authenticationToken?.Claims
			};

			await _localStorageService.SetAsync(nameof(AuthenticationTokenWithClaims),
				authenticationToken);

			NotifyAuthenticationStateChanged(Task.FromResult(
				new AuthenticationState(
					new CustomPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(authenticationToken.AccessToken ?? ""), "jwtAuthType")))));

			if (tokenResponse.Data is null) throw new NotFoundException(ErrorDefaults.NotFound.RemoteApiEmptyResponse);
		}

		public async Task<Response> SignOut(CancellationToken cancellationToken = default)
		{

			var authenticationToken = await _localStorageService.GetAsync<AuthenticationTokenWithClaims>(nameof(AuthenticationTokenWithClaims));
			if (!authenticationToken.Success||authenticationToken.Value is null)
			{
				return new ErrorResponse($"{nameof(AuthenticationToken)} not found", HttpStatusCode.NotFound);
			}

			await _identityServerService.RevokeRefreshToken(authenticationToken.Value.RefreshToken ?? "", false, cancellationToken);

			await _localStorageService.DeleteAsync(nameof(AuthenticationTokenWithClaims));

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new CustomPrincipal(new ClaimsIdentity()))));
			return new SuccessResponse();
		}

		public async Task<Response> SignIn(string username, string? password, CancellationToken cancellationToken = default)
		{
			var userResponse = await _identityServerService.SignIn(
				username, password??"", false, cancellationToken);

			if (!userResponse.Success)
				return new ErrorResponse(userResponse.Message, HttpStatusCode.BadRequest);

			if (userResponse.Data?.UserInfo is null)
				return new ErrorResponse(ErrorDefaults.NotFound.UserInfoNotFound, HttpStatusCode.NotFound);

			if (userResponse.Data?.Token is null)
				return new ErrorResponse(ErrorDefaults.NotFound.TokenNotFound, HttpStatusCode.NotFound);

			var authenticationToken = new AuthenticationTokenWithClaims()
			{
				AccessToken = userResponse.Data?.Token.AccessToken,
				RefreshToken = userResponse.Data?.Token.RefreshToken,
				Expired = DateTime.Now.AddSeconds(userResponse.Data?.Token.ExpiresIn ?? 0),
				Claims = JsonConvert.DeserializeObject<AuthenticationClaims>(userResponse.Data?.UserInfo.Raw??"") 
			};

			await _localStorageService.SetAsync(nameof(AuthenticationTokenWithClaims),
				authenticationToken);

			NotifyAuthenticationStateChanged(Task.FromResult(
				new AuthenticationState(
					new CustomPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(authenticationToken.AccessToken ?? ""), "jwtAuthType")))));
			return new SuccessResponse();
		}
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			AuthenticationTokenWithClaims? token;
			try
			{
				token = (await _localStorageService.GetAsync<AuthenticationTokenWithClaims>(nameof(AuthenticationTokenWithClaims))).Value;
			}
			catch(System.Security.Cryptography.CryptographicException ex)
			{
				await _localStorageService.DeleteAsync(nameof(AuthenticationTokenWithClaims));
				NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new CustomPrincipal(new ClaimsIdentity()))));
				return new AuthenticationState(new CustomPrincipal(new ClaimsIdentity()));
			}
			 

			if (token?.AccessToken == null || string.IsNullOrEmpty(token.AccessToken))
			{
				return new AuthenticationState(new CustomPrincipal(new ClaimsIdentity()));
			}

			if (token.Expired >= DateTime.Now)
				return new AuthenticationState(
					new CustomPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token.AccessToken), "jwtAuthType")));
			await GetAccessTokenByRefreshToken();
			token = (await _localStorageService.GetAsync<AuthenticationTokenWithClaims>(nameof(AuthenticationTokenWithClaims))).Value;
			return new AuthenticationState(new CustomPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token?.AccessToken ?? ""), "jwtAuthType")));

		}
	}
}
