using CNG.Http.Services;
using Microsoft.IdentityModel.JsonWebTokens;
using TradingBot.Backend.Gateway.API.Defaults;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Binance;
using TradingBot.Backend.Gateway.API.Responses;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Api.Binance
{
	public class BinanceAccountService:IBinanceAccountService
	{
		private readonly IHttpClientService _client;
		private readonly string _url;
		public BinanceAccountService(IHttpClientService client, ITokenService tokenService,EnvironmentModel env, IHttpContextAccessor httpContextAccessor)
		{
			_client = client;
			_client.SetClient(Client.ResourceOwnerPasswordClient);
			_url = $"{env.MicroServices?.Binance}{Defaults.Gateway.Binance.AccountService}";
			_client.SetBearerAuthentication(tokenService.GetClientCredentialToken().Result);
			_client.SetHeader(new Dictionary<string, string>()
			{
				{"X-User",httpContextAccessor.HttpContext?.Request.HttpContext.User.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Sub)?.Value??""}
			});
		}

		public async Task<Response<AccountInfoSpotDto>> GetAccountInfoSpotAsync(string apiKey,string secretKey, CancellationToken cancellationToken = default)
		{
			var response=await _client.GetAsync<AccountInfoSpotDto>(
				$"{_url}/Spot/Info?{nameof(apiKey)}={apiKey}&{nameof(secretKey)}={secretKey}",cancellationToken: cancellationToken);
			return response.Success
				? new SuccessResponse<AccountInfoSpotDto>(response.Data)
				: new ErrorResponse<AccountInfoSpotDto>(response.Message, response.StatusCode);
		}
		public async Task<Response<AccountInfoFuturesDto>> GetAccountInfoFuturesAsync(string apiKey,string secretKey,CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<AccountInfoFuturesDto>(
				$"{_url}/Futures/Info?{nameof(apiKey)}={apiKey}&{nameof(secretKey)}={secretKey}",cancellationToken: cancellationToken);
			return response.Success
				? new SuccessResponse<AccountInfoFuturesDto>(response.Data)
				: new ErrorResponse<AccountInfoFuturesDto>(response.Message, response.StatusCode);
		}
		public async Task<Response<List<AccountBalanceSpotDto>>> GetAccountBalanceSpotAsync(string apiKey, string secretKey, CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<List<AccountBalanceSpotDto>>(
				$"{_url}/Spot/Balance?{nameof(apiKey)}={apiKey}&{nameof(secretKey)}={secretKey}",cancellationToken: cancellationToken);
			return response.Success
				? new SuccessResponse<List<AccountBalanceSpotDto>>(response.Data)
				: new ErrorResponse<List<AccountBalanceSpotDto>>(response.Message, response.StatusCode);
		}
		public async Task<Response<List<AccountBalanceFuturesDto>>> GetAccountBalanceFuturesAsync(string apiKey, string secretKey, CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<List<AccountBalanceFuturesDto>>(
				$"{_url}/Futures/Balance?{nameof(apiKey)}={apiKey}&{nameof(secretKey)}={secretKey}",cancellationToken: cancellationToken);
			return response.Success
				? new SuccessResponse<List<AccountBalanceFuturesDto>>(response.Data)
				: new ErrorResponse<List<AccountBalanceFuturesDto>>(response.Message, response.StatusCode);
		}

	}
}
