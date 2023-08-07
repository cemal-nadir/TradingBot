using Binance.Net.Interfaces.Clients;
using CNG.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;
using TradingBot.Backend.Libraries.Domain.Defaults;
using TradingBot.Backend.Libraries.Infrastructure.Services.Binance;

namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services;

public class Infrastructure: IServiceInstaller
{
    public void InstallServices(IServiceCollection services, EnvironmentModel env)
    {
        services.AddHttpClientService();
        services.AddHttpClient(Client.DefaultClient);
        services.AddSingleton<IBinanceRestClient, BinanceTestClient>();
        services.AddScoped<IBinanceMainClient, BinanceMainClient>();
    }

}