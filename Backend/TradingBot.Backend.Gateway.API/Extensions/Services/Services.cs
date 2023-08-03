using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;
using TradingBot.Backend.Gateway.API.Services.Abstract.Token;
using TradingBot.Backend.Gateway.API.Services.Concrete.Gateway;
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
			services.AddScoped<IPlatformGateway, PlatformGateway>();

			#endregion

			#region Token

			services.AddScoped<ITokenService, TokenService>();

			#endregion
		}
	}
}
