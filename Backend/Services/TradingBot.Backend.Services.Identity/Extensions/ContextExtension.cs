using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;

namespace TradingBot.Backend.Services.Identity.Api.Extensions;

public static class ContextExtensions
{
    public static void AddClients(this ConfigurationDbContext context)
    {
        foreach (var client in Config.Clients.ToList())
        {
            context.Clients.Add(client.ToEntity());
        }

        context.SaveChanges();
    }
    public static void AddApiResources(this ConfigurationDbContext context)
    {
        foreach (var resource in Config.ApiResources.ToList())
        {
            context.ApiResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }
    public static void AddApiScopes(this ConfigurationDbContext context)
    {
        foreach (var scope in Config.ApiScopes.ToList())
        {
            context.ApiScopes.Add(scope.ToEntity());
        }
        context.SaveChanges();
    }
    public static void AddIdentityResources(this ConfigurationDbContext context)
    {
        foreach (var resource in Config.IdentityResources.ToList())
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }
}