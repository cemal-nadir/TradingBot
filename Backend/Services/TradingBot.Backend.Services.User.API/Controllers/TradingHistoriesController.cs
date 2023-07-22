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
		[HttpPost("LastOrder")]
		public async Task<IActionResult> GetLastOrderForSymbol([FromBody]GetLastOrderDto dto,CancellationToken cancellationToken=default)
		{
			return Ok(await _tradingHistoryService.GetLastOrderForSymbolAsync(dto.Symbol ?? "", dto.TradingAccountId ?? "",
				dto.OrderType, cancellationToken));
		}
	}
}
