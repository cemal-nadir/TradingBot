using CNG.Core.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace TradingBot.Frontend.Web.Blazor.Services
{
	public class BrowserService
	{
		private IJSRuntime? _jsRuntime;
		private ComponentBase? _component;

		public async void Init(IJSRuntime jsRuntime, ComponentBase component)
		{
			_jsRuntime ??= jsRuntime;
			_component ??= component;
		}
	}
}
