using Blazored.LocalStorage;
using CNG.Http.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Web.Blazor.Resources;

namespace TradingBot.Frontend.Web.Blazor.Shared
{
	public class MainLayoutRazor : LayoutComponentBase
	{
		[Inject] protected ILocalStorageService LocalStorageService { get; set; } = null!;
		[Inject] protected IStringLocalizer<Resource> Localizer { get; set; } = null!;
		[Inject] protected NavigationManager Navigation { get; set; } = null!;
		[Inject] protected ISnackbar Snackbar { get; set; } = null!;
		[Inject] protected IDialogService DialogService { get; set; } = null!;
		[Inject] protected IJSRuntime JsRuntime { get; set; } = null!;
		[Inject] private GlobalRenderService GlobalRenderService { get; set; } = null!;

		protected bool Loading { get; set; }
		protected bool ProfileMenuShowing { get; set; }

		protected void ProfileMenuClicked()
		{
			ProfileMenuShowing = !ProfileMenuShowing;
		}

		private void HandleError(object? sender,ExceptionResponse exceptionResponse)
		{

			Snackbar.Add($"{exceptionResponse.StatusCode} - {exceptionResponse.Message}({exceptionResponse.StackTrace})", Severity.Error, config =>
			{
				config.CloseAfterNavigation = true;
				config.ShowCloseIcon = true;
				config.BackgroundBlurred = true;
				config.SnackbarVariant = Variant.Filled;
			});
			StateHasChanged();
		}
	
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				GlobalRenderService.ExceptionRender+=HandleError;
			}

		}

	}
}
