using CNG.Http.Extensions;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Services;

namespace TradingBot.Frontend.Web.Blazor.Installers.Services;

public class Services : IServiceInstaller
{
    public void InstallServices(WebApplicationBuilder builder, Env environments)
    {
	    builder.Services.AddSingleton<GlobalRenderService>();
	    builder.Services.AddScoped<IIdentityService, IdentityService>();
		builder.Services.AddHttpClient(ClientDefaults.DefaultClient);
		builder.Services.AddHttpClientService();

        #region Api Services


		#endregion
	}
}