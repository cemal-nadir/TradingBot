using Hangfire;
using Hangfire.Logging.LogProviders;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MongoDB.Driver;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Services.Abstract.Hangfire;
using TradingBot.Backend.Gateway.API.Services.Concrete.Hangfire;

namespace TradingBot.Backend.Gateway.API.Extensions.Services;

public class HangFire : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {

        if (env is not { MongoDb: not null }) return;


        var mongoUrlBuilder = new MongoUrlBuilder(string.IsNullOrEmpty(env.MongoDb.ConnectionString)
            ? string.IsNullOrEmpty(env.MongoDb.UserName)
                ? $"mongodb://{env.MongoDb.Host}:{env.MongoDb.Port}"
                : $"mongodb://{env.MongoDb.UserName}:{env.MongoDb.Password}@{env.MongoDb.Host}:{env.MongoDb.Port}?authSource=admin"
            : $"{env.MongoDb.ConnectionString}")
        {
            DatabaseName = $"{env.Project.GroupName}Jobs"
        };

        var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
        services.AddTransient<IHangfireService, HangfireService>();
        services.AddHangfire(configuration =>
        {
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    Prefix = "hangfire",
                    CheckConnection = true
                });
        });
        GlobalConfiguration.Configuration.UseLogProvider(new ColouredConsoleLogProvider(Hangfire.Logging.LogLevel.Trace));

    }
}