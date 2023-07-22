using TradingBot.Backend.Gateway.API.Dtos.Requests.Hooks;

namespace TradingBot.Backend.Gateway.API.Services.Abstract.Gateway
{
	public interface IIndicatorHookGateway
	{
		Task HookAsync(string indicatorId, IndicatorHookDto dto, CancellationToken cancellationToken = default);
	}
}
