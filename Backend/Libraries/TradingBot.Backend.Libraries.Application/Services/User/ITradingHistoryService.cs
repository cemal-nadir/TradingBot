using CNG.Abstractions.Repositories;
using TradingBot.Backend.Libraries.Application.Dtos.User;

namespace TradingBot.Backend.Libraries.Application.Services.User
{
	public interface ITradingHistoryService:IServiceRepository<TradingHistoryDto,string>
	{
	}
}
