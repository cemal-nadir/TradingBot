using CNG.Core.Exceptions;
using CNG.Http.Responses;
using CNG.Http.Services;
using Microsoft.Extensions.Caching.Memory;
using TradingBot.Backend.Gateway.API.Defaults;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Dtos.Enums;
using TradingBot.Backend.Gateway.API.Models;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Token
{
	public class TokenService:ITokenService
	{
		private readonly IMemoryCache _memoryCache;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IIdentityServerService _identityServerService;
		private readonly EnvironmentModel _env;
		public TokenService(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, EnvironmentModel env, IIdentityServerService identityServerService)
		{
			_httpContextAccessor = httpContextAccessor;
			_memoryCache = memoryCache;
			_env = env;
			_identityServerService = identityServerService;
		}
		public async Task<string> GetClientCredentialToken(CancellationToken cancellationToken = default)
		{
			if (_httpContextAccessor.HttpContext?.User.Identity is null || !_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				return "";
			}

			if (_httpContextAccessor.HttpContext.User.IsInRole("admin"))
			{
				return await GetToken(AuthType.FullClient, cancellationToken);
			}

		

			if (_httpContextAccessor.HttpContext.User.IsInRole("user"))
			{
				return await GetToken(AuthType.UserClient, cancellationToken);
			}

			return "";
		}

		public string GetResourceOwnerPasswordToken(CancellationToken cancellationToken = default) =>
			_httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString().Replace("Bearer ", "") ?? "";

		private async Task<string> GetToken(AuthType authType = AuthType.UserClient, CancellationToken cancellationToken = default)
		{
			var currentTokens = GetCurrentTokens();
			var currentToken = currentTokens.FirstOrDefault(x => x.Type == authType);

			if (currentToken is not null && currentToken.ExpireTime >= DateTime.Now)
				return currentToken.Token ?? "";

			var clientId = authType switch
			{
				AuthType.UserClient => _env.Clients?.User?.Id ?? "",
				AuthType.FullClient => _env.Clients?.Full?.Id ?? "",
				_ => _env.Clients?.User?.Id ?? ""
			};
			var clientSecret = authType switch
			{
				AuthType.UserClient => _env.Clients?.User?.Secret ?? "",
				AuthType.FullClient => _env.Clients?.Full?.Secret ?? "",
				_ => _env.Clients?.User?.Secret ?? ""
			};

			var token = await GetNewToken(clientId, clientSecret, cancellationToken);
			_memoryCache.Set(authType.ToString(), new ApiToken
			{
				Id = clientId,
				Secret = clientSecret,
				Token = token.AccessToken,
				ExpireTime = token.EndTime,
				Type = authType
			});

			return token.AccessToken ?? "";
		}
		private async Task<ClientCredentialResponse> GetNewToken(string clientId, string clientSecret, CancellationToken cancellationToken = default)
		{
			_identityServerService.SetClient(_env.MicroServices?.Identity ?? "", clientId, clientSecret, Client.DefaultClient);
			var response =
				await _identityServerService.GetClientCredentialTokenAsync(cancellationToken: cancellationToken);
			if (!response.Success) throw new BadRequestException(response.Message);

			if (response.Data is null) throw new NotFoundException(Error.NotFound.RemoteApiEmptyResponse);

			return response.Data;
		}
		private IEnumerable<ApiToken> GetCurrentTokens()
		{
			return (from object? key in Enum.GetValues(typeof(AuthType))
					select _memoryCache.Get<ApiToken>(key.ToString() ?? "")
				into token
					where token != null
					select token).ToList();
		}
	}
}
