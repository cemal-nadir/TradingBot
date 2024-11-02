using CNG.Http.Services;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Repositories;
using TradingBot.Backend.Gateway.API.Responses;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Api.User
{
	public class TradingAccountService : ServiceRepository<string, TradingAccountDto, TradingAccountsDto>, ITradingAccountService
	{
		private readonly IHttpClientService _client;
		private readonly string _url;

		public TradingAccountService(IHttpClientService client, EnvironmentModel env, IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : base(client, $"{env.MicroServices?.User}{Defaults.Gateway.User.TradingAccountService}", Defaults.Client.DefaultClient, httpContextAccessor, tokenService)
		{
			_client = client;
			_url = $"{env.MicroServices?.User}{Defaults.Gateway.User.TradingAccountService}";
		}

		public async Task<Response<List<TradingAccountsDto>>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<List<TradingAccountsDto>>($"{_url}/User/{userId}", cancellationToken: cancellationToken);
			return response.Success
				? new SuccessResponse<List<TradingAccountsDto>>(response.Data)
				: new ErrorResponse<List<TradingAccountsDto>>(response.Message, response.StatusCode);
		}

		public async Task<Response<TradingAccountsDto>> GetByIndicatorIdAsync(string indicatorId, CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<TradingAccountsDto>($"{_url}/Indicator/{indicatorId}", cancellationToken: cancellationToken);
			return response.Success
				? new SuccessResponse<TradingAccountsDto>(response.Data)
				: new ErrorResponse<TradingAccountsDto>(response.Message, response.StatusCode);
		}
	}
}
