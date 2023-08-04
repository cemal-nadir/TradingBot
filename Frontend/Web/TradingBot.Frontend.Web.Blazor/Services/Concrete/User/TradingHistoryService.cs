using CNG.Http.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Services.Concrete.User
{
	public class TradingHistoryService: ServiceRepository<string, TradingHistoryDto, TradingHistoriesDto>, ITradingHistoryService
	{
		public TradingHistoryService(IHttpClientService client, Env env, ProtectedLocalStorage localStorage) : base(client, $"{env.GatewayUrl}{ServiceDefaults.User.UserService}",$"TradingHistory",localStorage)
		{
		}
	}
}
