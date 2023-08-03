using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlatformsController : ControllerBase
	{
		private readonly IPlatformGateway _platformGateway;

		public PlatformsController(IPlatformGateway platformGateway)
		{
			_platformGateway = platformGateway;
		}


		[HttpGet("{tradingPlatform}/Balance")]
		public async Task<IActionResult> GetBalance([FromRoute] string tradingPlatform, [FromQuery]string apiKey,[FromQuery]string secretKey, CancellationToken cancellationToken = default)
		{
			var balance =
				await _platformGateway.GetCurrentBalance(apiKey, secretKey, tradingPlatform, cancellationToken);
			return Ok(balance);
		}
	}
}
