using CNG.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components
{
    public class MultipleSelectListDialogRazor: ComponentBase
    {
        [Inject] protected IStringLocalizer<Resource> Localizer { get; set; } = null!;
        [CascadingParameter] public MudDialogInstance? MudDialog { get; set; }
        [Parameter] public List<SelectList<string>> Items { get; set; } = new();
        [Parameter] public Color Color { get; set; } = Color.Primary;
        [Parameter] public string Icon { get; set; } = Icons.Material.Filled.Save;
        [Parameter] public string ButtonText { get; set; } = "Ok";
        [Parameter] public string Placeholder { get; set; } = string.Empty;
        [Parameter] public string Label { get; set; } = string.Empty;
        [Parameter] public IEnumerable<string>? SelectedValues { get; set; }

        protected string GetMultiSelectionText(IEnumerable<string> selectedValues)
        {
            return selectedValues.Aggregate("", (current, item) => current + ((current == "" ? "" : ", ") + Items.FirstOrDefault(x => x.Id==item)?.Description));
        }
        protected void Submit() => MudDialog?.Close(DialogResult.Ok(SelectedValues));
    }
}
