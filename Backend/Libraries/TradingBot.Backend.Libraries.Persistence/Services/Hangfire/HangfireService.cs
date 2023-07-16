using DotNetCore.CAP;
using TradingBot.Backend.Libraries.Application.Services.Hangfire;

namespace TradingBot.Backend.Libraries.Persistence.Services.Hangfire
{
	public class HangfireService:IHangfireService
	{
		// ReSharper disable once NotAccessedField.Local
		private readonly ICapPublisher _capPublisher;

		public HangfireService(ICapPublisher capPublisher)
		{
			_capPublisher = capPublisher;
		}
	}
}
