using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using TradingBot.Frontend.Web.Blazor.Components.Bases;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using TradingBot.Frontend.Libraries.Blazor;

namespace TradingBot.Frontend.Web.Blazor.Theme.Components
{
	public class ProfileMenuRazor : BaseComponent
	{
		[Inject] public ProtectedLocalStorage ProtectedLocalStorage { get; set; } = null!;
		[Inject] public IIdentityService IdentityService { get; set; } = null!;


		protected AuthenticationClaims? Claims { get; set; }
		protected readonly List<CultureInfo> SupportedCultures = new()
		{
			new CultureInfo("tr-TR"),
			new CultureInfo("en-US"),
			new CultureInfo("de-DE")
		};
		protected void ChangeCulture(CultureInfo cultureInfo)
		{
			if (Equals(CultureInfo.CurrentCulture, cultureInfo)) return;

			var uri = new Uri(Navigation.Uri)
				.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
			var culture = Uri.EscapeDataString(cultureInfo.Name);
			var redirectionUri = Uri.EscapeDataString(uri);

			Navigation.NavigateTo($"/api/v1/auth/setCulture?culture={culture}&redirectionUri={redirectionUri}", forceLoad: true);
		}
		protected async Task Logout()
		{
			await IdentityService.SignOut();
			Navigation.NavigateTo("/login", forceLoad: true);
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				try
				{
					Claims = (await ProtectedLocalStorage.GetAsync<AuthenticationTokenWithClaims>(nameof(AuthenticationTokenWithClaims))).Value?.Claims;
				}
				catch (System.Security.Cryptography.CryptographicException ex)
				{
					Navigation.NavigateTo("/login", forceLoad: true);
				}
				StateHasChanged();
			}

		}
	}
}
