using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Components.Bases;

public class BaseComponent : ComponentBase
{
	[Inject] protected ILocalStorageService LocalStorageService { get; set; } = null!;
	[Inject] protected IStringLocalizer<Resource> Localizer { get; set; } = null!;
	[Inject] protected NavigationManager Navigation { get; set; } = null!;
	[Inject] protected ISnackbar Snackbar { get; set; } = null!;
	[Inject] protected IDialogService DialogService { get; set; } = null!;
	protected bool Loading { get; set; }
	protected bool DialogShowing { get; set; }
}
