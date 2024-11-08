using CNG.MongoDB.Configuration;
using CNG.MongoDB.Extensions;
using TradingBot.Backend.Gateway.API.Dtos;

namespace TradingBot.Backend.Gateway.API.Extensions.Services;

public class MongoDb : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        if (env.MongoDb == null) return;
        var connectionString = string.IsNullOrEmpty(env.MongoDb.ConnectionString)
                ? string.IsNullOrEmpty(env.MongoDb.UserName)
                    ? $"mongodb://{env.MongoDb.Host}:{env.MongoDb.Port}"
                    : $"mongodb://{env.MongoDb.UserName}:{env.MongoDb.Password}@{env.MongoDb.Host}:{env.MongoDb.Port}"
                : env.MongoDb.ConnectionString
            ;

        services.AddMongoDb(new MongoDbRepositoryOptions
        {
            ConnectionString = connectionString,
            DbName = $"{env.Project.GroupName}{env.Project.ProjectName}API",
            CollectionNamingConvention = NamingConvention.Pascal
        });
    }
}