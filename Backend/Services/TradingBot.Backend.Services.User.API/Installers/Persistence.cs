using TradingBot.Backend.Libraries.ApiCore.Installers;
using TradingBot.Backend.Libraries.Application;
using TradingBot.Backend.Libraries.Application.Repositories.User;
using TradingBot.Backend.Libraries.Application.Services.User;
using TradingBot.Backend.Libraries.Persistence.Repositories.User;
using TradingBot.Backend.Libraries.Persistence.Services.User;

namespace TradingBot.Backend.Services.User.API.Installers
{
	public class Persistence: IServiceInstaller
	{
		public void InstallServices(IServiceCollection services, EnvironmentModel env)
		{
			services.AddScoped<ITradingAccountRepository, TradingAccountRepository>();
			services.AddScoped<ITradingHistoryRepository, TradingHistoryRepository>();
			services.AddScoped<ITradingAccountService, TradingAccountService>();
			services.AddScoped<ITradingHistoryService, TradingHistoryService>();
		}
	}
}
