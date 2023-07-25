using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Libraries.ApiCore.Repositories;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Services.User;

namespace TradingBot.Backend.Services.User.API.Controllers
{
	public class TradingHistoriesController:ControllerRepository<ITradingHistoryService,TradingHistoryDto,string>
	{
		private readonly ITradingHistoryService _tradingHistoryService;
		public TradingHistoriesController(ITradingHistoryService service, ITradingHistoryService tradingHistoryService) : base(service)
		{
			_tradingHistoryService = tradingHistoryService;
		}
		[Authorize(Policy = "FullOrRead")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPost("LastOrder")]
		public async Task<IActionResult> GetLastOrderForSymbol([FromBody]GetLastOrderDto dto,CancellationToken cancellationToken=default)
		{
			return Ok(await _tradingHistoryService.GetLastOrderForSymbolAsync(dto.Symbol ?? "", dto.TradingAccountId ?? "",
				dto.OrderType, cancellationToken));
		}
	}
}
