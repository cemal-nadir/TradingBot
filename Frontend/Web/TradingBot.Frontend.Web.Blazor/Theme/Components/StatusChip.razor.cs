using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components;


public class StatusChipRazor : ComponentBase
{
    
    [Parameter] public bool Status { get; set; }
    [Parameter] public bool Label { get; set; } 

    [Parameter] public string? ActiveTitle { get; set; } 
    [Parameter] public Color ActiveColor { get; set; } = Color.Info;
    [Parameter] public string ActiveIcon { get; set; } = Icons.Material.Filled.VerifiedUser;
    [Parameter] public Variant ActiveVariant { get; set; } = Variant.Outlined;


    [Parameter] public string? InactiveTitle { get; set; } 
    [Parameter] public Color InactiveColor { get; set; } = Color.Secondary;
    [Parameter] public string InactiveIcon { get; set; } = Icons.Material.Filled.Close;
    [Parameter] public Variant InactiveVariant { get; set; } = Variant.Outlined;
    [Parameter] public Size Size { get; set; } 
}