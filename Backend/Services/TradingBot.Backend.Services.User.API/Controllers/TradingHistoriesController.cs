using TradingBot.Backend.Libraries.ApiCore.Repositories;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Services.User;

namespace TradingBot.Backend.Services.User.API.Controllers
{
	public class TradingHistoriesController:ControllerRepository<ITradingHistoryService,TradingHistoryDto,string>
	{
		public TradingHistoriesController(ITradingHistoryService service) : base(service)
		{
		}
	}
}
