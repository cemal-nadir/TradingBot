using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Services
{
	public class CustomProfileService : IProfileService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
		public CustomProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
		{
			_userManager = userManager;
			_claimsFactory = claimsFactory;
		}
		public async Task IsActiveAsync(IsActiveContext context)
		{
			var user=await _userManager.FindByIdAsync(context.Subject.GetSubjectId());
			context.IsActive = user is { IsBanned: false };
		}
		public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
		{
			var sub = context.Subject?.GetSubjectId()?? throw new Exception("No sub claim present");
			await GetProfileDataAsync(context, sub);
		}
		protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, string subjectId)
		{
			var user = await FindUserAsync(subjectId);
			if (user != null)
			{
				await GetProfileDataAsync(context, user);
			}
		}
		protected virtual async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
		{
			var principal = await GetUserClaimsAsync(user);
			context.AddRequestedClaims(principal.Claims);
			context.IssuedClaims.AddRange(principal.Claims);
			context.IssuedClaims=context.IssuedClaims.Distinct().ToList();
		}
		protected virtual async Task<ClaimsPrincipal> GetUserClaimsAsync(ApplicationUser user)
		{
			var principal = await _claimsFactory.CreateAsync(user)?? throw new Exception("ClaimsFactory failed to create a principal");

			return principal;
		}
		protected virtual async Task<ApplicationUser?> FindUserAsync(string subjectId)
		{
			return await _userManager.FindByIdAsync(subjectId);
		}
	}
}
