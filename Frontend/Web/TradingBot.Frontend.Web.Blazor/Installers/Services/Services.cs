using CNG.Http.Extensions;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.TradingPlatforms;
using TradingBot.Frontend.Web.Blazor.Services.Abstract.User;
using TradingBot.Frontend.Web.Blazor.Services.Concrete.TradingPlatforms;
using TradingBot.Frontend.Web.Blazor.Services.Concrete.User;

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

        builder.Services.AddScoped<ITradingAccountService, TradingAccountService>();
        builder.Services.AddScoped<ITradingHistoryService, TradingHistoryService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ITradingPlatformService, TradingPlatformService>();
        #endregion
    }
}