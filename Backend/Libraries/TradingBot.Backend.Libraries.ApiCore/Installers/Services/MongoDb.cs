using CNG.MongoDB.Configuration;
using CNG.MongoDB.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;

namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services;

public class MongoDb : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        if (env.MongoDb == null || string.IsNullOrEmpty(env.MongoDb.Host)) return;
        var connectionString = string.IsNullOrEmpty(env.MongoDb.ConnectionString)
                ? string.IsNullOrEmpty(env.MongoDb.UserName)
                    ? $"mongodb://{env.MongoDb.Host}:{env.MongoDb.Port}"
                    : $"mongodb://{env.MongoDb.UserName}:{env.MongoDb.Password}@{env.MongoDb.Host}:{env.MongoDb.Port}"
                : env.MongoDb.ConnectionString
            ;

        services.AddMongoDb(new MongoDbRepositoryOptions
        {
            ConnectionString = connectionString,//$"mongodb://{env.MongoDb.UserName}:{env.MongoDb.Password}@{env.MongoDb.Host}:{env.MongoDb.Port}",
            DbName = $"{env.Project?.GroupName}{env.Project?.ProjectName}",
            CollectionNamingConvention = NamingConvention.Pascal
        });
    }
}