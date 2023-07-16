using Hangfire;
using Hangfire.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;
using TradingBot.Backend.Libraries.Application.Services.Hangfire;
using TradingBot.Backend.Libraries.Persistence.Services.Hangfire;


namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services;

public class HangFire : IServiceInstaller, IConfigureInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        services.AddTransient<IHangfireService, HangfireService>();
        services.AddHangfire(x => x.UseInMemoryStorage());
        services.AddHangfireServer();
    }

    public void InstallConfigures(IApplicationBuilder app)
    {
        GlobalConfiguration.Configuration.UseInMemoryStorage(new InMemoryStorageOptions());
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });
        app.UseHangfireDashboard("/job", new DashboardOptions());

        // Zamanlanmýþ Görev Ayarlarý "Benzersiz Görev Kimliði",Çalýþacak Metod,"Cron Pattern" Örnek Altta

        // RecurringJob.AddOrUpdate<IHangFireService>("TestMethod", x => x.TestMethodAsync(default), "*/30 * * * *");
    }
}