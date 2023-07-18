using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Libraries.ApiCore.Repositories;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Services.User;

namespace TradingBot.Backend.Services.User.API.Controllers
{
	
	public class TradingAccountsController:ControllerRepository<ITradingAccountService,TradingAccountDto,string>
	{
		private readonly ITradingAccountService _service;
		public TradingAccountsController(ITradingAccountService service) : base(service)
		{
			_service = service;
		}

		[HttpGet("{indicatorId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIndicatorId([FromQuery] string indicatorId,
			CancellationToken cancellationToken = default)
		{
			return Ok(await _service.GetByIndicatorId(indicatorId, cancellationToken));
		}
	}
}
