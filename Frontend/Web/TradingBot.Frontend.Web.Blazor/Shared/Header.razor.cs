using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Web.Blazor.Components.Bases;

namespace TradingBot.Frontend.Web.Blazor.Shared
{
	public class HeaderRazor : BaseComponent
	{
		[Parameter] public EventCallback<bool> ProfileMenuOnClick { get; set; }
        [Inject] private IIdentityService IdentityService { get; set; } = null!;
        protected IList<string> UserRoles = new List<string>();
        protected bool OpenMenu { get; set; } = true;
		protected void ToggleMenuDrawer()
		{
			OpenMenu = !OpenMenu;
		}
		protected void ToggleProfileMenuDrawer()
		{
			if (!ProfileMenuOnClick.HasDelegate) return;
			ProfileMenuOnClick.InvokeAsync();
		}
        protected override async Task OnInitializedAsync()
        {
            var authState = await IdentityService.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity is { IsAuthenticated: true })
            {
                UserRoles = user.Claims
                    .Where(c => c.Type =="role")
                    .Select(c => c.Value)
                    .ToList();
            }
        }

    }
}
