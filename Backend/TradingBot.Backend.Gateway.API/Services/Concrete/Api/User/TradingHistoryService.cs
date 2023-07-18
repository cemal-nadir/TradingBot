using CNG.Http.Services;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Repositories;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Api.User
{
    public class TradingHistoryService : ServiceRepository<string, TradingHistoryDto, TradingHistoriesDto>, ITradingHistoryService
    {
        public TradingHistoryService(IHttpClientService client, EnvironmentModel env, IHttpContextAccessor httpContextAccessor, ITokenService tokenService) : base(client, $"{env.MicroServices?.User}{Defaults.Gateway.User.TradingHistoryService}", Defaults.Client.DefaultClient, httpContextAccessor, tokenService)
        {
        }
    }
}
