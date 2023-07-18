using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks;

namespace TradingBot.Backend.Gateway.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class IndicatorHooksController:ControllerBase
	{
		[HttpPost("{indicatorId}")]
		public async Task<IActionResult> HookAsync([FromRoute]string indicatorId,IndicatorHookDto dto, CancellationToken cancellationToken = default)
		{
			return Ok();
		}
	}
}
