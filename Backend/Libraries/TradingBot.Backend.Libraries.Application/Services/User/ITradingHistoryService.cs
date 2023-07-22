using CNG.Abstractions.Repositories;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Libraries.Application.Services.User
{
	public interface ITradingHistoryService:IServiceRepository<TradingHistoryDto,string>
	{
		Task<TradingHistoriesDto?> GetLastOrderForSymbolAsync(string symbol, string tradingAccountId,
			OrderType orderType,
			CancellationToken cancellationToken = default);
	}
}
