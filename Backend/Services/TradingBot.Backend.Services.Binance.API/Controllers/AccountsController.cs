using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Libraries.Application.Services.Infrastructure.Binance;

namespace TradingBot.Backend.Services.Binance.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController:ControllerBase
	{
		private readonly IBinanceMainClient _binanceMainClient;

		public AccountsController(IBinanceMainClient binanceMainClient)
		{
			_binanceMainClient = binanceMainClient;
		}

		[HttpGet("Spot/Info")]
		public async Task<IActionResult> GetAccountInfoSpot(string apiKey,string secretKey, CancellationToken cancellationToken = default)
		{
			_binanceMainClient.SetClient(apiKey,secretKey);
			return Ok(await _binanceMainClient.BinanceAccountService.GetAccountInfoSpot(cancellationToken));
		}
		[HttpGet("Futures/Info")]
		public async Task<IActionResult> GetAccountInfoFutures(string apiKey,string secretKey, CancellationToken cancellationToken = default)
		{
			_binanceMainClient.SetClient(apiKey, secretKey);
			return Ok(await _binanceMainClient.BinanceAccountService.GetAccountInfoFutures(cancellationToken));
		}
		[HttpGet("Spot/Balance")]
		public async Task<IActionResult> GetAccountBalanceSpot(string apiKey, string secretKey, CancellationToken cancellationToken = default)
		{
			_binanceMainClient.SetClient(apiKey, secretKey);
			return Ok(await _binanceMainClient.BinanceAccountService.GetAccountBalanceSpot(cancellationToken));
		}
		[HttpGet("Futures/Balance")]
		public async Task<IActionResult> GetAccountBalanceFutures(string apiKey, string secretKey, CancellationToken cancellationToken = default)
		{
			_binanceMainClient.SetClient(apiKey, secretKey);
			return Ok(await _binanceMainClient.BinanceAccountService.GetAccountBalanceFutures(cancellationToken));
		}
	}
}
