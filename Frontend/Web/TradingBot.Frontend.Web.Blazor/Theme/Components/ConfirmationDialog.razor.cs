using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class ConfirmationDialogRazor : ComponentBase
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }

    [Parameter] public string ContentText { get; set; } = string.Empty;

    [Parameter] public string OkButtonText { get; set; } = string.Empty;
    [Parameter] public string CancelButtonText { get; set; } = string.Empty;

    [Parameter] public Color Color { get; set; } = Color.Info;

    protected void Submit() => MudDialog?.Close(DialogResult.Ok(true));
    protected void Cancel() => MudDialog?.Cancel();
}