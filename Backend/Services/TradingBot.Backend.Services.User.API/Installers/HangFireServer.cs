using CNG.Core;
using Hangfire;
using TradingBot.Backend.Libraries.ApiCore.Installers;
using TradingBot.Backend.Libraries.Application;
using TradingBot.Backend.Libraries.Application.Services.Hangfire;

namespace TradingBot.Backend.Services.User.API.Installers
{
    public class HangFireServer:IServiceInstaller,IConfigureInstaller
    {
        public void InstallConfigures(IApplicationBuilder app)
        {
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 1 });

            var env = ServiceTool.ServiceProvider?.GetRequiredService<EnvironmentModel>();

            app.UseHangfireDashboard("/job-dashboard", new DashboardOptions()
            {
                DarkModeEnabled = true,
                DashboardTitle = "Trading Bot Zamanlanmış Görevler",
                DisplayStorageConnectionString = true,
            });

            RecurringJob.AddOrUpdate<IHangfireService>("ReCalculateAdjustedBalances", x => x.ReCalculateAdjustedBalances(default), "0 0 * * *");
        }

        public void InstallServices(IServiceCollection services, EnvironmentModel env)
        {
            services.AddHangfireServer();
        }
    }
}
