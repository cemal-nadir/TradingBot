using CNG.Cap;
using CNG.Core;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;

namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services;

public class Cap : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        if (env is not { MongoDb: not null, RabbitMq: not null }) return;
        services.AddCapService(
            options: new CapOption(
                groupName: $"{env.Project?.GroupName}",
                version: "v4",
                mongoOption: new CapOption.Mongo(
                    string.IsNullOrEmpty(env.MongoDb.ConnectionString)
                        ? string.IsNullOrEmpty(env.MongoDb.UserName)
                            ? $"mongodb://{env.MongoDb.Host}:{env.MongoDb.Port}"
                            : $"mongodb://{env.MongoDb.UserName}:{env.MongoDb.Password}@{env.MongoDb.Host}:{env.MongoDb.Port}"
                        : env.MongoDb.ConnectionString,
                    $"{env.Project?.GroupName}Cap"
                ),
                rabbitMqOption: new CapOption.RabbitMq(
                    host: env.RabbitMq.Host,
                    userName: env.RabbitMq.UserName,
                    password: env.RabbitMq.Password,
                    port: env.RabbitMq.Port
                ),
                dashboardUrl: "/cap-dashboard"
            )
            {
                FailedRetryCount = 1
            });
        ServiceTool.Create(services);
    }
}