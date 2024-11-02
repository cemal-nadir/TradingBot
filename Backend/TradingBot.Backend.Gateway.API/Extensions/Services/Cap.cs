using CNG.Cap;
using CNG.Core;
using TradingBot.Backend.Gateway.API.Dtos;

namespace TradingBot.Backend.Gateway.API.Extensions.Services;

public class Cap : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        if (env is not { MongoDb: not null, RabbitMq: not null }) return;
        services.AddCapService(
            new CapOption(
                $"{env.Project?.GroupName}",
                "v4",
                new CapOption.Mongo(
                    string.IsNullOrEmpty(env.MongoDb.ConnectionString)
                        ? string.IsNullOrEmpty(env.MongoDb.UserName)
                            ? $"mongodb://{env.MongoDb.Host}:{env.MongoDb.Port}"
                            : $"mongodb://{env.MongoDb.UserName}:{env.MongoDb.Password}@{env.MongoDb.Host}:{env.MongoDb.Port}"
                        : env.MongoDb.ConnectionString,
                    $"{env.Project?.GroupName}Cap"
                ),
                new CapOption.RabbitMq(
                    env.RabbitMq.Host,
                    env.RabbitMq.UserName,
                    env.RabbitMq.Password,
                    env.RabbitMq.Port
                ),
                "/cap-dashboard"
            )
            {
                FailedRetryCount = 1
            });
        ServiceTool.Create(services);

    }
}