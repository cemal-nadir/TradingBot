using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController:ControllerBase
	{
		private readonly IUserGateway _userGateway;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UsersController(IUserGateway userGateway, IHttpContextAccessor httpContextAccessor)
		{
			_userGateway = userGateway;
			_httpContextAccessor = httpContextAccessor;
		}

		#region TradingAccount

		#region GET

		[HttpGet("TradingAccount")]
		public async Task<IActionResult> TradingAccounts(CancellationToken cancellationToken = default)
		{
			return Ok(await _userGateway.GetTradingAccountsAsync(cancellationToken));
		}


		#endregion

		#region POST

		[HttpPost("TradingAccount")]
		public async Task<IActionResult> InsertTradingAccount(TradingAccountDto dto, CancellationToken cancellationToken = default)
		{
			dto.UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "";
			await _userGateway.InsertTradingAccountAsync(dto, cancellationToken);
			return CreatedAtAction(null, null);
		}
		[HttpPost("{id}/TradingAccount")]
		public async Task<IActionResult> InsertTradingAccount([FromRoute] string id, TradingAccountDto dto, CancellationToken cancellationToken = default)
		{
			dto.UserId = id;
			await _userGateway.InsertTradingAccountAsync(dto, cancellationToken);
			return CreatedAtAction(null, null);
		}

		#endregion

		#region PUT

		[HttpPut("TradingAccount/{tradingAccountId}")]
		public async Task<IActionResult> UpdateTradingAccount([FromRoute]string tradingAccountId,TradingAccountDto dto, CancellationToken cancellationToken = default)
		{
			dto.UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "";
			await _userGateway.UpdateTradingAccountAsync(tradingAccountId,dto, cancellationToken);
			return StatusCode(204);
		}
		[HttpPut("{id}/TradingAccount/{tradingAccountId}")]
		public async Task<IActionResult> UpdateTradingAccountByUserId([FromRoute] string id,[FromRoute]string tradingAccountId, TradingAccountDto dto, CancellationToken cancellationToken = default)
		{
			dto.UserId = id;
			await _userGateway.UpdateTradingAccountAsync(tradingAccountId, dto, cancellationToken);
			return StatusCode(204);
		}

		#endregion

		#region DELETE

		[HttpDelete("TradingAccount")]
		public async Task<IActionResult> DeleteTradingAccount([FromRoute]string tradingAccountId,CancellationToken cancellationToken = default)
		{
			await _userGateway.DeleteTradingAccountAsync(tradingAccountId,cancellationToken);
			return StatusCode(204);
		}
		[HttpDelete("TradingAccount")]
		public async Task<IActionResult> TradingAccountsByUserId([FromRoute] List<string> tradingAccountIds,CancellationToken cancellationToken=default)
		{
			await _userGateway.DeleteRangeTradingAccountsAsync(tradingAccountIds,cancellationToken);
			return StatusCode(204);
		}

		#endregion

		#endregion
	}
}
