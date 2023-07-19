using TradingBot.Backend.Gateway.API.Dtos.Requests.Binance;
using TradingBot.Backend.Gateway.API.Responses;

namespace TradingBot.Backend.Gateway.API.Services.Abstract.Api.Binance
{
	public interface IBinanceAccountService
	{
		Task<Response<AccountInfoSpotDto>> GetAccountInfoSpotAsync(string apiKey, string secretKey,
			CancellationToken cancellationToken = default);

		Task<Response<AccountInfoFuturesDto>> GetAccountInfoFuturesAsync(string apiKey, string secretKey,
			CancellationToken cancellationToken = default);

		Task<Response<List<AccountBalanceSpotDto>>> GetAccountBalanceSpotAsync(string apiKey, string secretKey,
			CancellationToken cancellationToken = default);

		Task<Response<List<AccountBalanceFuturesDto>>> GetAccountBalanceFuturesAsync(string apiKey, string secretKey,
			CancellationToken cancellationToken = default);
	}
}
