using Microsoft.AspNetCore.Components;
using System.Web;
using MudBlazor;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Web.Blazor.Dtos.Account;
using Microsoft.AspNetCore.Components.Authorization;

namespace TradingBot.Frontend.Web.Blazor.Pages.Account
{
	public class LoginRazor : BaseComponent
	{
		[Inject] public IIdentityService IdentityService { get; set; } = null!;
		[Inject] protected LoginValidator Validator { get; set; } = null!;
		[CascadingParameter] public Task<AuthenticationState>? AuthState { get; set; }
		protected LoginDto LoginDto { get; set; } = new();
		protected MudForm Form = new();
		protected bool Processing { get; set; }
		protected bool ShowPassword { get; set; }
		protected new bool Loading { get; set; } = true;
		protected async Task OnValidSubmitAsync()
		{
			Processing = true;
			StateHasChanged();
			await Form.Validate();
			if (Form.IsValid)
			{
				var response = await IdentityService.SignIn(LoginDto.Email ?? "", LoginDto.Password ?? "");
				if (!response.Success)
					Snackbar.Add(Localizer[response.Message], Severity.Error);
				else
					Navigation.NavigateTo($"/{ReturnUrl()}",forceLoad:true);
			}
			Processing = false;
			StateHasChanged();

		}

		protected override async Task OnParametersSetAsync()
		{
			if (AuthState != null)
			{
				var authState = await AuthState;
				if (authState.User.Identity is { IsAuthenticated: true })
				{
					Navigation.NavigateTo("/", forceLoad: true);
				}
			}

			Loading = false;
			StateHasChanged();
			await base.OnParametersSetAsync();
		}

		
		
		
		
		private string ReturnUrl()
		{
			var absoluteUri = new Uri(Navigation.Uri);
			var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
			return queryParam["returnUrl"] ?? "";
		}
	}
}
