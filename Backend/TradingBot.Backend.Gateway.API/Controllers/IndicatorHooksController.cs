using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class IndicatorHooksController:ControllerBase
	{
		private readonly IIndicatorHookGateway _indicatorHookGateway;

		public IndicatorHooksController(IIndicatorHookGateway indicatorHookGateway)
		{
			_indicatorHookGateway = indicatorHookGateway;
		}

		[HttpPost("{indicatorId}")]
		public async Task<IActionResult> HookAsync([FromRoute]string indicatorId,IndicatorHookDto dto, CancellationToken cancellationToken = default)
		{
			await _indicatorHookGateway.HookAsync(indicatorId, dto, cancellationToken);
			return Ok();
		}
	}
}
