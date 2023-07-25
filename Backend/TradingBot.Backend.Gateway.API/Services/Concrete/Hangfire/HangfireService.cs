using DotNetCore.CAP;
using TradingBot.Backend.Gateway.API.Services.Abstract.Hangfire;

namespace TradingBot.Backend.Gateway.API.Services.Concrete.Hangfire
{
	public class HangfireService:IHangfireService
	{
		private readonly ICapPublisher _capPublisher;

		public HangfireService(ICapPublisher capPublisher)
		{
			_capPublisher = capPublisher;
		}
	}
}
