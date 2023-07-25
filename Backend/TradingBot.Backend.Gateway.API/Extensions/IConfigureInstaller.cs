namespace TradingBot.Backend.Gateway.API.Extensions;

public interface IConfigureInstaller
{
    /// <summary>
    ///     Install Configure
    /// </summary>
    /// <param name="app"></param>
    void InstallConfigures(IApplicationBuilder app); 
}