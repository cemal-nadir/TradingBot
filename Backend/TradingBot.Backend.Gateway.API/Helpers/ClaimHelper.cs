using IdentityModel;
using System.Security.Claims;
using System.Security.Principal;

namespace TradingBot.Backend.Gateway.API.Helpers
{
	public static class ClaimHelper
	{
		public static bool CustomIsInRole(this IPrincipal principal, string role)
		{
			return (ClaimsIdentity?)principal.Identity is not null &&
				   ((ClaimsIdentity)principal.Identity).HasClaim(JwtClaimTypes.Role, role);
		}

	}
}
