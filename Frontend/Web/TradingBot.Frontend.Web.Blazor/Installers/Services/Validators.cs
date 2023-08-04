using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Web.Blazor.Dtos.Account;
using TradingBot.Frontend.Web.Blazor.Dtos.Identity;
using TradingBot.Frontend.Web.Blazor.Dtos.Users;

namespace TradingBot.Frontend.Web.Blazor.Installers.Services
{
	public class Validators:IServiceInstaller
	{
		public void InstallServices(WebApplicationBuilder builder, Env environments)
		{
			builder.Services.AddSingleton<LoginValidator>();
			builder.Services.AddSingleton<TradingAccountValidator>();
			builder.Services.AddSingleton<UserValidator>();
		}
	}
}
