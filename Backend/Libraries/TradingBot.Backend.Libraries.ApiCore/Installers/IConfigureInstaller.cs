using Microsoft.AspNetCore.Builder;

namespace TradingBot.Backend.Libraries.ApiCore.Installers;

public interface IConfigureInstaller
{
    /// <summary>
    ///     Install Configure
    /// </summary>
    /// <param name="app"></param>
    void InstallConfigures(IApplicationBuilder app); 
}