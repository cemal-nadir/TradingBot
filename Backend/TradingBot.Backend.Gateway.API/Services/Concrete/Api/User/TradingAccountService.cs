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
		public TradingAccountService(IHttpClientService client, EnvironmentModel env, IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : base(client, $"{env.MicroServices?.User}{Defaults.Gateway.User.TradingAccountService}", Defaults.Client.DefaultClient, httpContextAccessor, tokenService)
		{
			_client = client;
		}
		public async Task<Response<TradingAccountsDto>> GetByIndicatorIdAsync(string indicatorId, CancellationToken cancellationToken = default)
		{
			var response = await _client.GetAsync<TradingAccountsDto>($"?indicatorId={indicatorId}", cancellationToken);
			return response.Success
				? new SuccessResponse<TradingAccountsDto>(response.Data)
				: new ErrorResponse<TradingAccountsDto>(response.Message, response.StatusCode);
		}
	}
}
