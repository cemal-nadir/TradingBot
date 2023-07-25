using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;

public class SwitchButtonRazor : ComponentBase
{
    [Parameter] public bool Checked { get; set; }
    [Parameter] public bool ReadOnly { get; set; }
    [Parameter] public string? CheckedText { get; set; }
    [Parameter] public string? UnCheckedText { get; set; }
    [Parameter] public Color Color { get; set; } = Color.Success;
    [Parameter] public Color UnCheckedColor { get; set; } = Color.Error;
    [Parameter] public string Class { get; set; } = string.Empty;
}