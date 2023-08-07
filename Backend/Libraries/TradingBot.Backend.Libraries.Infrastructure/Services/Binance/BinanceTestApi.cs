using Binance.Net.Clients;
using Binance.Net.Enums;

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
