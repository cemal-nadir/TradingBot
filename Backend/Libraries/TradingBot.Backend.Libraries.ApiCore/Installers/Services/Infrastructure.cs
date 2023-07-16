using CNG.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;
using TradingBot.Backend.Libraries.Domain.Defaults;

namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services;

public class Infrastructure: IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        services.AddHttpClientService();
        services.AddHttpClient(Client.DefaultClient);
    }

}