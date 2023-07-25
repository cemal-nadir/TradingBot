using CNG.Http.Extensions;
using TradingBot.Backend.Gateway.API.Defaults;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Identity;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Concrete.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Concrete.Api.Identity;
using TradingBot.Backend.Gateway.API.Services.Concrete.Api.User;

namespace TradingBot.Backend.Gateway.API.Extensions.Services
{
	public class HttpServices:IServiceInstaller
	{
		public void InstallServices(IServiceCollection services, EnvironmentModel env)
		{
			services.AddHttpClientService();
			services.AddHttpClient(Client.DefaultClient);
			#region Api

			#region Binance

			services.AddScoped<IBinanceAccountService, BinanceAccountService>();

			#endregion

			#region User

			services.AddScoped<ITradingAccountService, TradingAccountService>();
			services.AddScoped<ITradingHistoryService, TradingHistoryService>();

			#endregion

			#region Identity

			services.AddScoped<IUserService, UserService>();

			#endregion

			#endregion
		}
	}
}
