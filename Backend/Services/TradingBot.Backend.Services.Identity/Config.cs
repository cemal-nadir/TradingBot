using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace TradingBot.Backend.Services.Identity.Api;

public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
	        new ApiScope("TradingBotUserApi.read","Scope TradingBot User Api Read Permission"),
            new ApiScope("TradingBotUserApi.write","Scope TradingBot User Api Write Permission"),
            new ApiScope("TradingBotUserApi.full","Scope TradingBot User Api Full Permission"),
            new ApiScope("TradingBotBinanceApi.read","Scope TradingBot Binance Api Read Permission"),
            new ApiScope("TradingBotBinanceApi.write","Scope TradingBot Binance Api Write Permission"),
            new ApiScope("TradingBotBinanceApi.full","Scope TradingBot Binance Api Full Permission"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };
    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
      
        new("TradingBotUserApi"){Scopes={ "TradingBotUserApi.read", "TradingBotUserApi.write", "TradingBotUserApi.full" } },
        new("TradingBotBinanceApi"){Scopes={ "TradingBotBinanceApi.read", "TradingBotBinanceApi.full" } },
        new(IdentityServerConstants.LocalApi.ScopeName)
    };
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Phone(),
            new("roles", new[] { JwtClaimTypes.Role }),
        };
    public static IEnumerable<Client> Clients =>
        new[]
        {
            #region Api Clients

            new Client
            {
                ClientName = "TradingBot Api Full Client",
                ClientId = "TradingBotApiFullClient",
                ClientSecrets = { new Secret("cv7VzznOzv8RrqLaaSJA4jZ5GeB3TvM8".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
					"TradingBotUserApi.full", "TradingBotBinanceApi.full",
                    IdentityServerConstants.LocalApi.ScopeName
                }
            },
            new Client
            {
                ClientName = "TradingBot Api Member Client",
                ClientId = "TradingBotApiMemberClient",
                ClientSecrets = { new Secret("q9bLfoMsKLsH8v7c234lugrn9l0LQFYV".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes =
                {
					 "TradingBotUserApi.read", "TradingBotUserApi.write", "TradingBotBinanceApi.read",

					IdentityServerConstants.LocalApi.ScopeName
                }
            },
         

            #endregion

            #region User Clients

            new Client
            {
                ClientName = "TradingBot Blazor Web Client",
                ClientId = "TradingBotBlazorWebClient",
                AllowOfflineAccess = true,
                ClientSecrets = { new Secret("SmNK7ieDbESioB597WrV3ptABnGDYqCK".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.Phone,
                    IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName,
                    "roles"
                },
                AccessTokenLifetime = 1 * 60 * 60,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                RefreshTokenUsage = TokenUsage.ReUse
            }

            #endregion


        };
}