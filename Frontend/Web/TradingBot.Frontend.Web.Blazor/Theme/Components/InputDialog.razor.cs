using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class InputDialogRazor : ComponentBase
{
    [Inject] protected IStringLocalizer<Resource> Localizer { get; set; } = null!;
    
    [CascadingParameter] protected MudDialogInstance? MudDialog { get; set; }

    [Parameter] public Color Color { get; set; } = Color.Primary;
    [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Save;

    [Parameter] public string ButtonText { get; set; } = "Ok";
    [Parameter] public string Placeholder { get; set; } = string.Empty;
    [Parameter] public string? Value { get; set; }

    protected void Submit() => MudDialog?.Close(DialogResult.Ok(Value));
}