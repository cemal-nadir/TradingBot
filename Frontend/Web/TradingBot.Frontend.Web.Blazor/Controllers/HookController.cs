using CNG.Http.Services;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Frontend.Libraries.Blazor.Defaults;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Web.Blazor.Dtos.Hooks;

namespace TradingBot.Frontend.Web.Blazor.Controllers
{
	[Route("/api/v1/hook")]
	public class HookController:Controller
	{
		private readonly IHttpClientService _client;
		private readonly string _url;
		public HookController(IHttpClientService client, Env env)
		{
			_url = $"{env.GatewayUrl}{ServiceDefaults.IndicatorHook.IndicatorHookService}";
			_client = client;
			_client.SetClient(ClientDefaults.DefaultClient);
		}

		[HttpPost("{indicatorId}")]
		public async Task<IActionResult> IndicatorHook([FromRoute] string indicatorId, [FromBody] IndicatorHookDto dto,
			CancellationToken cancellationToken = default)
		{
			await _client.PostAsync($"{_url}/{indicatorId}", dto, cancellationToken);
			return Ok();
		}
	}
}
