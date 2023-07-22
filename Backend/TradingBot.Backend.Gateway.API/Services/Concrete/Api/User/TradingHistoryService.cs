using CNG.Http.Services;
using Newtonsoft.Json;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Dtos.Enums;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Repositories;
using TradingBot.Backend.Gateway.API.Responses;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Api.User
{
    public class TradingHistoryService : ServiceRepository<string, TradingHistoryDto, TradingHistoriesDto>, ITradingHistoryService
    {
	    private readonly IHttpClientService _client;
	    private readonly string _url;
        public TradingHistoryService(IHttpClientService client, EnvironmentModel env, IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : base(client, $"{env.MicroServices?.User}{Defaults.Gateway.User.TradingHistoryService}", Defaults.Client.DefaultClient, httpContextAccessor, tokenService)
        {
	        _client = client;
	        _url = $"{env.MicroServices?.Binance}{Defaults.Gateway.User.TradingHistoryService}";
		}

        public async Task<Response<TradingHistoriesDto>> GetLastOrder(string symbol, string tradingAccountId, OrderType orderType,
	        CancellationToken cancellationToken = default)
        {
	       var response=await _client.PostAsync($"{_url}/LastOrder",
		        new GetLastOrderDto { OrderType = orderType, Symbol = symbol, TradingAccountId = tradingAccountId },
		        cancellationToken);
	       if (!response.Success) return new ErrorResponse<TradingHistoriesDto>(response.Message, response.StatusCode);

	       return new SuccessResponse<TradingHistoriesDto>(!string.IsNullOrEmpty(response.Message)
		       ? JsonConvert.DeserializeObject<TradingHistoriesDto>(response.Message)
		       : new TradingHistoriesDto());
        }
    }
}
