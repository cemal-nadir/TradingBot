using DotNetCore.CAP;
using TradingBot.Backend.Libraries.Domain.Defaults;

namespace TradingBot.Backend.Services.Binance.API.Caps
{
	public class PullSubscribeService:ICapSubscribe
	{
		[CapSubscribe(Cap.BinanceHook)]
		public async Task HookAsync(CancellationToken cancellationToken = default)
		{

		}
	}
}
