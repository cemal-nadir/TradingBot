using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class CarouselDialogRazor : ComponentBase
{
    [CascadingParameter] MudDialogInstance? MudDialog { get; set; }
    [Parameter] public Transition Transition { get; set; } = Transition.Slide;
    [Parameter] public List<string> Images { get; set; } = new();

}