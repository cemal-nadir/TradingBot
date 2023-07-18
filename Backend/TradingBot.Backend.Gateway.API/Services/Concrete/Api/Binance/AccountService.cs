using CNG.Http.Services;
using TradingBot.Backend.Gateway.API.Defaults;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Api.Binance
{
	public class AccountService:IAccountService
	{
		private readonly IHttpClientService _client;
		private readonly string _url;
		public AccountService(IHttpClientService client, ITokenService tokenService,EnvironmentModel env)
		{
			_client = client;
			_client.SetClient(Client.DefaultClient);
			_url = $"{env.MicroServices?.Binance}{Defaults.Gateway.Binance.AccountService}";
			_client.SetBearerAuthentication(tokenService.GetToken().Result);
		}
		
	}
}
