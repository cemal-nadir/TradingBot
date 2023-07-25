using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using TradingBot.Backend.Services.Identity.Api.Exceptions;
using TradingBot.Backend.Services.Identity.Api.Middlewares;
using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Services;

public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly UserManager<ApplicationUser> _userManager;
    public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
	    var existUser = await _userManager.FindByEmailAsync(context.UserName);

        var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);
        if (!passwordCheck) throw new BadRequestException(ErrorDefaults.BadRequest.Password);
            
        context.Result = new GrantValidationResult(existUser.Id, OidcConstants.AuthenticationMethods.Password);
    }
}