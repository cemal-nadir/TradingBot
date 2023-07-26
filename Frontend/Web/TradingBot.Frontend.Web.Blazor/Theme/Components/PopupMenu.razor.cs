using Microsoft.AspNetCore.Components;
using TradingBot.Frontend.Libraries.Blazor.Models;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class PopupMenuRazor : ComponentBase
{
    [Parameter] public MenuModel? Menu { get; set; }
    [Parameter] public EventCallback<NavMenuOnClickResult> NavMenuOnClick { get; set; }

    protected void NavMenuOnClicked(NavMenuOnClickResult result)
    {
        if (!NavMenuOnClick.HasDelegate) return;
        NavMenuOnClick.InvokeAsync(result);
    }
}
