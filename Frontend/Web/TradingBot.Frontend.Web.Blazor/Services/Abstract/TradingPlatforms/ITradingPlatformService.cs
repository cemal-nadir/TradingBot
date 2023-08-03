using TradingBot.Frontend.Libraries.Blazor.Responses;
using TradingBot.Frontend.Web.Blazor.Dtos.Enums;

namespace TradingBot.Frontend.Web.Blazor.Services.Abstract.TradingPlatforms
{
	public interface ITradingPlatformService
	{
		Task<Response<decimal>> GetAccountBalance(string apiKey, string secretKey, TradingPlatform tradingPlatform,
			CancellationToken cancellationToken = default);
	}
}
