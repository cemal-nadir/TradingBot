

namespace TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance
{
	public interface IBinanceMainClient
	{
		void SetClient(string apiKey, string secretKey);
		IBinanceAccountService BinanceAccountService { get; }
		IBinanceOrderService BinanceOrderService { get; }
	}
}
