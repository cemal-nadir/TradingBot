using System.Security.Claims;
using System.Security.Principal;
using IdentityModel;

namespace TradingBot.Frontend.Libraries.Blazor
{
	public class CustomPrincipal:ClaimsPrincipal
	{
		public CustomPrincipal(IIdentity identity) : base(identity)
		{
		}

		public override bool IsInRole(string role)
		{
			return HasClaim(JwtClaimTypes.Role, role);
		}
	}
}
