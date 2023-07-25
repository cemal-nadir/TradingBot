using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Abstract.Api.User;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;
using TradingBot.Backend.Gateway.API.Services.Abstract.Hangfire;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;
using TradingBot.Backend.Gateway.API.Services.Concrete.Api.Binance;
using TradingBot.Backend.Gateway.API.Services.Concrete.Api.User;
using TradingBot.Backend.Gateway.API.Services.Concrete.Gateway;
using TradingBot.Backend.Gateway.API.Services.Concrete.Hangfire;
using TradingBot.Backend.Gateway.API.Services.Concrete.Token;

namespace TradingBot.Backend.Gateway.API.Extensions.Services
{
	public class Services:IServiceInstaller
	{
		public void InstallServices(IServiceCollection services, EnvironmentModel env)
		{

			#region Gateway

			services.AddScoped<IIndicatorHookGateway, IndicatorHookGateway>();
			services.AddScoped<IUserGateway, UserGateway>();

			#endregion

			#region Token

			services.AddScoped<ITokenService, TokenService>();

			#endregion
		}
	}
}
