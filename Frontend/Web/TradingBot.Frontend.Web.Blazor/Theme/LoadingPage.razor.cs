using Microsoft.AspNetCore.Components;
using TradingBot.Frontend.Web.Blazor.Components.Bases;

namespace TradingBot.Frontend.Web.Blazor.Theme;

public class LoadingPageRazor : BaseComponent
{
	[Parameter] public bool Visible { get; set; }
    [Parameter] public bool InComponent { get; set; } = true;
}