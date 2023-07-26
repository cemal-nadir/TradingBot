using TradingBot.Frontend.Libraries.Blazor.Models;

namespace TradingBot.Frontend.Web.Blazor.Installers;

public interface IServiceInstaller
{
	/// <summary>
	///     Install Service Collection
	/// </summary>
	/// <param name="builder"></param>
	/// <param name="environments"></param>
	void InstallServices(WebApplicationBuilder builder,Env environments);
}