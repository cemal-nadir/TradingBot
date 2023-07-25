using CNG.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class SelectListDialogRazor : ComponentBase
{
    [Inject] protected IStringLocalizer<Resource> Localizer { get; set; } = null!;
    [CascadingParameter] public MudDialogInstance? MudDialog { get; set; }
    [Parameter] public List<SelectList<string>> Items { get; set; } = new();
    [Parameter] public Color Color { get; set; } = Color.Primary;
    [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Save;
    [Parameter] public string ButtonText { get; set; } = "Ok";
    [Parameter] public string Placeholder { get; set; } = string.Empty;

    protected string? Value { get; set; }
    protected void Submit() => MudDialog?.Close(DialogResult.Ok(Value));
}