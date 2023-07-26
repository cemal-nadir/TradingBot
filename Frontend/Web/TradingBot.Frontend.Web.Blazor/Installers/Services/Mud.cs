using MudBlazor;
using MudBlazor.Services;
using TradingBot.Frontend.Libraries.Blazor.Models;

namespace TradingBot.Frontend.Web.Blazor.Installers.Services;

public class Mud : IServiceInstaller
{
    public void InstallServices(WebApplicationBuilder builder, Env environments)
    {
        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
    }
}