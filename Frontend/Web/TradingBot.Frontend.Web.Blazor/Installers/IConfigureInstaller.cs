namespace TradingBot.Frontend.Web.Blazor.Installers;

public interface IConfigureInstaller
{
    /// <summary>
    ///     Install Configure
    /// </summary>
    /// <param name="app"></param>
    void InstallConfigures(WebApplication app); 
}