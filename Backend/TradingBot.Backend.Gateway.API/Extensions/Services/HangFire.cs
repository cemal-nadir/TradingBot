using Hangfire;
using Hangfire.InMemory;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Services.Abstract.Hangfire;
using TradingBot.Backend.Gateway.API.Services.Concrete.Hangfire;

namespace TradingBot.Backend.Gateway.API.Extensions.Services;

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