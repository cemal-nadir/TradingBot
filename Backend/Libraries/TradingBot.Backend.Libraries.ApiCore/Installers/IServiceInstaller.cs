using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;

namespace TradingBot.Backend.Libraries.ApiCore.Installers;

public interface IServiceInstaller
{
    /// <summary>
    ///     Install Service Collection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="env"></param>
    void InstallServices(IServiceCollection services,EnvironmentModel env);
}