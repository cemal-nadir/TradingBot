using CNG.Cache;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;

namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services;

public class Redis : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        if (env.Redis != null)
        {
            services.AddRedisService(new RedisOption(
                instanceName: $"{env.Project?.ProjectName}",
                identityName: $"IdentityServer",
                connectionString: $"{env.Redis.Host}:{env.Redis.Port},ssl=False",
                absoluteExpiration: 60));
        }
    }
}