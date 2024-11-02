using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Libraries.ApiCore.Repositories;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Services.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Services.User.API.Controllers
{
	
	public class TradingAccountsController:ControllerRepository<ITradingAccountService,TradingAccountDto,string>
	{
		private readonly ITradingAccountService _service;
		public TradingAccountsController(ITradingAccountService service) : base(service)
		{
			_service = service;
		}
		[Authorize(Policy = "Full")]
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAll(
			CancellationToken cancellationToken = default)
		{
			return Ok(await _service.GetAllAsync(cancellationToken));
		}
		[Authorize(Policy = "FullOrRead")]
		[HttpGet(nameof(User)+"/{userId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		public async Task<IActionResult> GetAllByUserId([FromRoute]string userId,
			CancellationToken cancellationToken = default)
		{
			return Ok(await _service.GetAllByUserIdAsync(userId,cancellationToken));
		}
		[Authorize(Policy = "FullOrRead")]
		[HttpGet(nameof(TradingAccount.Indicator)+"/{indicatorId}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByIndicatorId([FromRoute] string indicatorId,
			CancellationToken cancellationToken = default)
		{
			return Ok(await _service.GetByIndicatorId(indicatorId, cancellationToken));
		}
	}
}
