using Microsoft.AspNetCore.Components;
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
