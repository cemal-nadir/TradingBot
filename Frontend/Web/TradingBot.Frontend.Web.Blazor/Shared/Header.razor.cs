using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Services;
using TradingBot.Frontend.Web.Blazor.Components.Bases;

namespace TradingBot.Frontend.Web.Blazor.Shared
{
	public class HeaderRazor : BaseComponent
	{
		[Parameter] public EventCallback<bool> ProfileMenuOnClick { get; set; }
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
		
	}
}
