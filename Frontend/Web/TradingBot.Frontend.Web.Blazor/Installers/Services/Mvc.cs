using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Models;

namespace TradingBot.Frontend.Web.Blazor.Installers.Services;

public class Mvc : IServiceInstaller, IConfigureInstaller
{
        
    public void InstallServices(WebApplicationBuilder builder,Env environments)
    {
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor(config =>
        {
	        config.DetailedErrors = true;
        });
        builder.Services.AddTransient<ProtectedSessionStorage>();
        builder.WebHost.UseWebRoot("wwwroot");
        builder.WebHost.UseStaticWebAssets();
       
    }

    public void InstallConfigures(WebApplication app)
    {
        
        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapBlazorHub();
        app.MapControllers();
        app.MapFallbackToPage("/_Host");

	}
}