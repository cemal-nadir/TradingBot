using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using TradingBot.Frontend.Libraries.Blazor.Models;
using TradingBot.Frontend.Libraries.Blazor.Services;

namespace TradingBot.Frontend.Web.Blazor.Installers.Services;

public class Authentication : IServiceInstaller
{
    public void InstallServices(WebApplicationBuilder builder, Env environments)
    {
		builder.Services.AddAuthentication(sharedOptions =>
		{
			sharedOptions.DefaultAuthenticateScheme =
				CookieAuthenticationDefaults.AuthenticationScheme;
			sharedOptions.DefaultSignInScheme =
				CookieAuthenticationDefaults.AuthenticationScheme;
			sharedOptions.DefaultChallengeScheme =
				OpenIdConnectDefaults.AuthenticationScheme;
		}).AddOpenIdConnect(opts =>
		{
			opts.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			opts.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
			opts.Authority = environments.IdentityUrl;
			opts.ClientId = environments.Client.Id;
			opts.ClientSecret = environments.Client.Secret;
			opts.SaveTokens = true;
			opts.RequireHttpsMetadata = false;
			opts.ClaimActions.MapJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role);
			opts.Scope.Add("email");
			opts.Scope.Add("phone");
			opts.Scope.Add("openid");
			opts.Scope.Add("profile");
			opts.Scope.Add("offline_access");
			opts.Scope.Add("IdentityServerApi");
			opts.GetClaimsFromUserInfoEndpoint = true;
			opts.TokenValidationParameters = new()
			{
				NameClaimType = "phone",
				RoleClaimType = "role",
			};
			opts.MapInboundClaims = false;
			opts.Events = new OpenIdConnectEvents
			{
				OnAccessDenied = context =>
				{
					context.HandleResponse();
					context.Response.Redirect("/");
					return Task.CompletedTask;
				}
			};
		});
		builder.Services.AddScoped<ProtectedSessionStorage>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityService>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();

	}


}