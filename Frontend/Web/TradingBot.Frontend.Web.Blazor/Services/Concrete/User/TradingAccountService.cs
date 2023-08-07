using CNG.Http.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Repositories;
using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;

namespace TradingBot.Frontend.Web.Blazor.Services.Concrete.User;

	public class TradingAccountService:ServiceRepository<string,TradingAccountDto,TradingAccountsDto>,ITradingAccountService
	{
		private readonly IHttpClientService _httpClientService;
		public TradingAccountService(IHttpClientService client, Env env,ProtectedLocalStorage localStorage, IHttpClientService httpClientService) : base(client, $"{env.GatewayUrl}{ServiceDefaults.User.UserService}",$"TradingAccount",localStorage)
		{
			_httpClientService = httpClientService;
		}

		public async Task<Response<List<TradingAccountsDto>>> GetTradingAccountsByUserId(string userId, CancellationToken cancellationToken = default)
		{
			_httpClientService.SetBearerAuthentication(await GetAccessToken());
			var response=await _httpClientService.GetAsync<List<TradingAccountsDto>>(
				$"{BaseUrl}/{userId}/{ServiceUrl}", cancellationToken);
			return response.Success
				? new SuccessResponse<List<TradingAccountsDto>>(response.Data)
				: new ErrorResponse<List<TradingAccountsDto>>(response.Message, response.StatusCode);
		}

	}

