using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingBot.Backend.Services.Identity.Api.Dtos;
using TradingBot.Backend.Services.Identity.Api.Services;
using static IdentityServer4.IdentityServerConstants;

namespace TradingBot.Backend.Services.Identity.Api.Controllers;

[Authorize(LocalApi.PolicyName)]
[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    #region CRUD
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] List<string> listOfId,CancellationToken cancellationToken=default)
	{
        return Ok(!listOfId.Any() ? await _userService.GetAllAsync() : await _userService.GetUsersByIdsAsync(listOfId, cancellationToken));
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("Search")]
    public async Task<IActionResult> GetAllByNameSurname([FromQuery] string searchText,CancellationToken cancellationToken=default)
    {
	    return Ok(await _userService.GetAllByNameSurname(searchText, cancellationToken));
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        return Ok(await _userService.GetAsync(id));
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> InsertUser(UserInsertDto dto)
    {
        return Ok(await _userService.InsertAsync(dto));
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UserUpdateDto dto)
    {
        await _userService.UpdateAsync(dto);
        return CreatedAtAction(null, null);
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _userService.DeleteAsync(id);
        return CreatedAtAction(null, null);
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("CurrentUser")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
        if (userIdClaim is null) return Unauthorized();

        return Ok(await _userService.GetAsync(userIdClaim.Value));
    }

    #endregion

    #region Roles
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("Roles")]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _userService.GetAllRoles());
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}/Roles")]
    public async Task<IActionResult> GetUserRoles(string id)
    {
        return Ok(await _userService.GetUserRolesAsync(id));
    }


    #endregion

    #region User Confirmation
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{id}/GenerateUserConfirmationToken")]
    public async Task<IActionResult> GenerateUserConfirmToken([FromRoute] string id)
    {
        return Ok(await _userService.GenerateUserConfirmationToken(id));
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{id}/ValidateUserConfirmation")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmUserToken([FromRoute] string id, [FromQuery] string token)
    {
        await _userService.ValidateUserConfirmationToken(id, token);
        return CreatedAtAction(null,null);
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
    public async Task<IActionResult> GeneratePassWordResetToken([FromRoute] string id)
    {
        return Ok(await _userService.GeneratePasswordResetToken(id));
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("{id}/ResetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromRoute] string id, [FromBody] ResetPasswordDto dto)
    {
        await _userService.PasswordReset(id, dto);

        return CreatedAtAction(null,null);
    }
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

        if (userIdClaim is null) return Unauthorized();

        await _userService.ChangePassword(userIdClaim.Value, dto);

        return CreatedAtAction(null, null);
    }

    #endregion






}