using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Web.Blazor.Dtos.Account;

namespace TradingBot.Frontend.Web.Blazor.Installers.Services
{
	public class Validators:IServiceInstaller
	{
		public void InstallServices(WebApplicationBuilder builder, Env environments)
		{
			builder.Services.AddSingleton<LoginValidator>();
		}
	}
}
