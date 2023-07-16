using AutoMapper;
using AutoMapper.Internal;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;
using TradingBot.Backend.Libraries.Application.Mappings;

namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services;

public class Mapper : IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        services.AddSingleton(new MapperConfiguration(cfg =>
        {
            cfg.Internal().MethodMappingEnabled = false;
            cfg.AddProfile(new AllProfiles());
        }).CreateMapper());
    }
}