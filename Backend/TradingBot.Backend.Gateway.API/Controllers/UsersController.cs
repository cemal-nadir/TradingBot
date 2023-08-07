using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Identity;
using TradingBot.Backend.Gateway.API.Dtos.Requests.Users;
using TradingBot.Backend.Gateway.API.Services.Abstract.Gateway;

namespace TradingBot.Backend.Gateway.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserGateway _userGateway;

		public UsersController(IUserGateway userGateway)
		{
			_userGateway = userGateway;
		}

		#region CRUD
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpGet]
		public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken=default)
		{
			return Ok(await _userGateway.GetAllUsers(cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpGet("Search")]
		public async Task<IActionResult> GetAllByNameSurname([FromQuery] string? searchText, CancellationToken cancellationToken = default)
		{
			return Ok(await _userGateway.GetAllByNameSurname(searchText, cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(string id,CancellationToken cancellationToken=default)
		{
			return Ok(await _userGateway.GetUser(id,cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpPost]
		public async Task<IActionResult> InsertUser(UserDto dto,CancellationToken cancellationToken=default)
		{
			await _userGateway.InsertUser(dto,cancellationToken);
			return CreatedAtAction(null, null);
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser([FromRoute]string id,UserDto dto, CancellationToken cancellationToken = default)
		{
			await _userGateway.UpdateUser(id,dto,cancellationToken);
			return CreatedAtAction(null, null);
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken = default)
		{
			await _userGateway.DeleteUser(id,cancellationToken);
			return CreatedAtAction(null, null);
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("CurrentUser")]
		public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken=default)
		{
			return Ok(await _userGateway.GetCurrentUser(cancellationToken));
		}

		#endregion

		#region Roles
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpGet("Roles")]
		public async Task<IActionResult> GetRoles(CancellationToken cancellationToken=default)
		{
			return Ok(await _userGateway.GetRoles(cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}/Roles")]
		public async Task<IActionResult> GetUserRoles(string id,CancellationToken cancellationToken=default)
		{
			return Ok(await _userGateway.GetUserRoles(id, cancellationToken));
		}


		#endregion

		#region User Confirmation
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPost("{id}/GenerateUserConfirmationToken")]
		public async Task<IActionResult> GenerateUserConfirmToken([FromRoute] string id,CancellationToken cancellationToken=default)
		{
			return Ok(await _userGateway.GenerateUserConfirmToken(id,cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPost("{id}/ValidateUserConfirmation")]
		[AllowAnonymous]
		public async Task<IActionResult> ConfirmUserToken([FromRoute] string id, [FromQuery] string token,CancellationToken cancellationToken=default)
		{
			await _userGateway.ConfirmUserToken(id, token,cancellationToken);
			return CreatedAtAction(null, null);
		}

		#endregion

		#region Password Reset
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPost("{id}/GeneratePasswordResetToken")]
		[AllowAnonymous]
		public async Task<IActionResult> GeneratePassWordResetToken([FromRoute] string id, CancellationToken cancellationToken = default)
		{
			return Ok(await _userGateway.GeneratePassWordResetToken(id,cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPost("{id}/ResetPassword")]
		[AllowAnonymous]
		public async Task<IActionResult> ResetPassword([FromRoute] string id, [FromBody] ResetPasswordDto dto,CancellationToken cancellationToken=default)
		{
			await _userGateway.ResetPassword(id, dto, cancellationToken);

			return CreatedAtAction(null, null);
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPost("ChangePassword")]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto,CancellationToken cancellationToken=default)
		{

			await _userGateway.ChangePassword(dto,cancellationToken);

			return CreatedAtAction(null, null);
		}

		#endregion

		#region TradingAccount

		#region GET
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]

		[HttpGet("TradingAccount")]
		public async Task<IActionResult> TradingAccounts(CancellationToken cancellationToken = default)
		{
			return Ok(await _userGateway.GetTradingAccountsAsync(cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		[HttpGet("{id}/TradingAccount")]
		public async Task<IActionResult> TradingAccountsByUserId([FromRoute]string id,CancellationToken cancellationToken = default)
		{
			return Ok(await _userGateway.GetTradingAccountsByUserIdAsync(id,cancellationToken));
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]

		[HttpGet("TradingAccount/{tradingAccountId}")]
		public async Task<IActionResult> TradingAccountById([FromRoute] string tradingAccountId, CancellationToken cancellationToken = default)
		{
			return Ok(await _userGateway.GetTradingAccountAsync(tradingAccountId, cancellationToken));
		}

		#endregion

		#region POST
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[ProducesResponseType(StatusCodes.Status403Forbidden)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[HttpPost("TradingAccount")]
		//public async Task<IActionResult> InsertTradingAccount(TradingAccountDto dto, CancellationToken cancellationToken = default)
		//{
		//	dto.UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "";
		//	await _userGateway.InsertTradingAccountAsync(dto, cancellationToken);
		//	return CreatedAtAction(null, null);
		//}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPost("TradingAccount")]
		public async Task<IActionResult> InsertTradingAccount(TradingAccountDto dto, CancellationToken cancellationToken = default)
		{
			await _userGateway.InsertTradingAccountAsync(dto, cancellationToken);
			return CreatedAtAction(null, null);
		}

		#endregion

		#region PUT
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		//[ProducesResponseType(StatusCodes.Status403Forbidden)]
		//[ProducesResponseType(StatusCodes.Status400BadRequest)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//[HttpPut("TradingAccount/{tradingAccountId}")]
		//public async Task<IActionResult> UpdateTradingAccount([FromRoute] string tradingAccountId, TradingAccountDto dto, CancellationToken cancellationToken = default)
		//{
		//	dto.UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? "";
		//	await _userGateway.UpdateTradingAccountAsync(tradingAccountId, dto, cancellationToken);
		//	return StatusCode(204);
		//}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpPut("TradingAccount/{tradingAccountId}")]
		public async Task<IActionResult> UpdateTradingAccountByUserId([FromRoute] string tradingAccountId, TradingAccountDto dto, CancellationToken cancellationToken = default)
		{
			await _userGateway.UpdateTradingAccountAsync(tradingAccountId, dto, cancellationToken);
			return StatusCode(204);
		}

		#endregion

		#region DELETE
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpDelete("TradingAccount/{tradingAccountId}")]
		public async Task<IActionResult> DeleteTradingAccount([FromRoute] string tradingAccountId, CancellationToken cancellationToken = default)
		{
			await _userGateway.DeleteTradingAccountAsync(tradingAccountId, cancellationToken);
			return StatusCode(204);
		}
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpDelete("TradingAccount")]
		public async Task<IActionResult> DeleteTradingAccounts([FromQuery] List<string> listOfId, CancellationToken cancellationToken = default)
		{
			await _userGateway.DeleteRangeTradingAccountsAsync(listOfId, cancellationToken);
			return StatusCode(204);
		}

		#endregion

		#endregion
	
	}
}
