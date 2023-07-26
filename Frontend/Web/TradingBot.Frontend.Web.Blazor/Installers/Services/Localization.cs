using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using TradingBot.Frontend.Libraries.Blazor.Models;

namespace TradingBot.Frontend.Web.Blazor.Installers.Services;

public class Localization : IServiceInstaller, IConfigureInstaller
{
    public void InstallServices(WebApplicationBuilder builder, Env environments)
    {
        builder.Services.AddLocalization();
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new("tr-TR"),
                new("en-US"),
                new("de-DE")
            };
            options.DefaultRequestCulture = new RequestCulture("tr-TR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
    }

    public void InstallConfigures(WebApplication app)
    {
        var requestLocalizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>()?.Value;
        if (requestLocalizationOptions != null) app.UseRequestLocalization(requestLocalizationOptions);
      
    }
}