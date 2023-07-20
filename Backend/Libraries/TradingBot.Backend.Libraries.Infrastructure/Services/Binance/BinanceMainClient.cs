using Binance.Net.Interfaces.Clients;
using CryptoExchange.Net.Authentication;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;

namespace TradingBot.Backend.Libraries.Infrastructure.Services.Binance
{
	public class BinanceMainClient:IBinanceMainClient
	{
		private readonly IBinanceRestClient _binanceRestClient;

		public BinanceMainClient(IBinanceRestClient binanceRestClient)
		{
			_binanceRestClient = binanceRestClient;
		}

		public void SetClient(string apiKey, string secretKey)
		{
			_binanceRestClient.SetApiCredentials(new ApiCredentials(apiKey, secretKey));
		}

		public IBinanceAccountService BinanceAccountService =>
			_binanceAccountService ??= new BinanceAccountService(_binanceRestClient);
		private BinanceAccountService? _binanceAccountService;
		public IBinanceOrderService BinanceOrderService =>
			_binanceOrderService ??= new BinanceOrderService(_binanceRestClient);
		private BinanceOrderService? _binanceOrderService;
	}
}
