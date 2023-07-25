using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using TradingBot.Frontend.Web.Blazor.Components.Bases;

namespace TradingBot.Frontend.Web.Blazor.Shared
{
	public class HeaderRazor : BaseComponent
	{
		protected bool Open { get; set; }
		protected void ToggleDrawer()
		{
			Open = !Open;
		}
	}
}
