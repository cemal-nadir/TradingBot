using CNG.Core;
using Microsoft.AspNetCore.Components;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class MultipleSelectListRazor : ComponentBase
{
    [Parameter] public string Label { get; set; } = "";
    
    [Parameter] public List<SelectList<string>> Entities { get; set; } = new();
    [Parameter] public IEnumerable<string>? SelectedValues { get; set; }

    protected string GetMultiSelectionText(List<string> selectedValues)
    {
        return selectedValues.Aggregate("", (current, item) => current + ((current == "" ? "" : ", ") + Entities.FirstOrDefault(x => x.Id == item)?.Description));
    }
}