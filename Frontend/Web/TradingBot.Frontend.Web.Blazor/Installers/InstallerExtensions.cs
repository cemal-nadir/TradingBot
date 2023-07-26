using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Web.Blazor.Handlers;

namespace TradingBot.Frontend.Web.Blazor.Installers;

public static class InstallerExtensions
{
	public static void InstallAllServices(this WebApplicationBuilder builder)
	{
		
		var env = new Env(
			identityUrl: Environment.GetEnvironmentVariable(variable: "IDENTITY_URL") ?? "",
			gatewayUrl: Environment.GetEnvironmentVariable(variable: "GATEWAY_URL") ?? "",
			clientId: Environment.GetEnvironmentVariable("CLIENT_ID") ?? "",
			clientSecret: Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? ""
			
		);

		builder.Services.AddSingleton(env);

		var installers = typeof(Program).Assembly.ExportedTypes
			.Where(predicate: x => typeof(IServiceInstaller).IsAssignableFrom(c: x) && x is { IsInterface: false, IsAbstract: false })
			.OrderBy(keySelector: x => x.Name)
			.Select(selector: Activator.CreateInstance)
			.Cast<IServiceInstaller>().ToList();
		installers.ForEach(action: x => x.InstallServices(builder: builder,environments:env));
	}

	public static void UseAllConfigurations(this WebApplication app)
	{

		var installers = typeof(Program).Assembly.ExportedTypes
			.Where(x => typeof(IConfigureInstaller).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
			.OrderBy(x => x.Name)
			.Select(Activator.CreateInstance)
			.Cast<IConfigureInstaller>().ToList();

		installers.ForEach(x => x.InstallConfigures(app));
		app.UseMiddleware<ErrorHandlingMiddleware>();

	}
}