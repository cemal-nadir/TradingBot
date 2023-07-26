using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class SubmitButtonRazor : ComponentBase
{
	[Parameter] public EventCallback SubmitButtonOnClick { get; set; }
    [Inject] public IStringLocalizer<Resource> Localizer { get; set; } = null!;

    [Parameter] public bool Processing { get; set; }

    [Parameter] public string Text { get; set; } = string.Empty;

    [Parameter] public string ProcessingText { get; set; } = "Processing";

    [Parameter] public ButtonType ButtonType { get; set; } = ButtonType.Button;

    [Parameter] public Color Color { get; set; } = Color.Primary;

    [Parameter] public string Class { get; set; } = string.Empty;

    [Parameter] public Variant Variant { get; set; } = Variant.Filled;
    [Parameter] public bool FullWidth { get; set; }
    [Parameter] public Size Size { get; set; } = Size.Medium;
    [Parameter] public string Style { get; set; } = string.Empty;

    protected async Task Click()
    {
	    await SubmitButtonOnClick.InvokeAsync();
	}
}