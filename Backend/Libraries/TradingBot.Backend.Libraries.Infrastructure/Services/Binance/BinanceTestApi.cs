using Binance.Net.Clients;

namespace TradingBot.Backend.Libraries.Infrastructure.Services.Binance
{
	public class BinanceTestClient: BinanceRestClient
	{
		public BinanceTestClient():base(options =>
		{
			options.AutoTimestamp = true;
		})
		{
				
		}
	}
}
