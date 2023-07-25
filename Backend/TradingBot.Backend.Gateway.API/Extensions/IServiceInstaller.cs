using TradingBot.Backend.Gateway.API.Dtos;

namespace TradingBot.Backend.Gateway.API.Extensions;

public interface IServiceInstaller
{
    /// <summary>
    ///     Install Service Collection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    void InstallServices(IServiceCollection services,EnvironmentModel env);
}