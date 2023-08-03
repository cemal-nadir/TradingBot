using CNG.Http.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Web.Blazor.Dtos.Enums;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.TradingPlatforms;

namespace TradingBot.Frontend.Web.Blazor.Services.Concrete.TradingPlatforms
{
	public class TradingPlatformService:ITradingPlatformService
	{
		private readonly IHttpClientService _client;
		private readonly string _url;
		private readonly ProtectedLocalStorage _protectedLocalStorage;
		public TradingPlatformService(IHttpClientService client, Env env, ProtectedLocalStorage protectedLocalStorage)
		{
			_url = $"{env.GatewayUrl}{ServiceDefaults.TradingPlatform.TradingPlatformService}";
			_client = client;
			_client.SetClient(ClientDefaults.DefaultClient);
			_protectedLocalStorage = protectedLocalStorage;
		}

		private async Task<string> GetAccessToken()
		{
			var result =
				await _protectedLocalStorage.GetAsync<AuthenticationTokenWithClaims>(nameof(AuthenticationTokenWithClaims));
			return result.Value?.AccessToken ?? "";
		}
		public async Task<Response<decimal>> GetAccountBalance(string apiKey,string secretKey, TradingPlatform tradingPlatform, CancellationToken cancellationToken = default)
		{
			_client.SetBearerAuthentication(await GetAccessToken());
			var response = await _client.GetAsync<decimal>($"{_url}/{tradingPlatform.ToString()}/Balance?{nameof(apiKey)}={apiKey}&{nameof(secretKey)}={secretKey}", cancellationToken);
			return response.Success
			? new SuccessResponse<decimal>(response.Data)
				: new ErrorResponse<decimal>(response.Message, response.StatusCode);
		}
	}
}
