using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingBot.Backend.Services.Identity.Api.Data;
using TradingBot.Backend.Services.Identity.Api.Extensions;
using TradingBot.Backend.Services.Identity.Api.Helpers;
using TradingBot.Backend.Services.Identity.Api.Models;

namespace TradingBot.Backend.Services.Identity.Api.Installers;

public static class Seed
{
	public static void CreateDatabases(this IHost builder)
	{
		var configurationContext = builder.Services.GetService<ConfigurationDbContext>();
		var identityContext = builder.Services.GetService<AspNetIdentityDbContext>();
		var persistedContext = builder.Services.GetService<PersistedGrantDbContext>();

		if (identityContext is null || configurationContext is null || persistedContext is null)
			throw new Exception("Contexts not installed");
		try
		{
			identityContext.Database.Migrate();
			identityContext.Database.EnsureCreated();

		}
		catch
		{
			// ignored
		}

		try
		{
			configurationContext.Database.Migrate();
			configurationContext.Database.EnsureCreated();

		}
		catch
		{
			// ignored
		}

		try
		{
			persistedContext.Database.Migrate();
			persistedContext.Database.EnsureCreated();
		}
		catch
		{
			// ignored
		}
	}
	public static void PrepareConfig(this IHost builder)
	{
		var configurationContext = builder.Services.GetService<ConfigurationDbContext>();
		if (configurationContext == null) return;
		PrepareClients(configurationContext);
		PrepareIdentityResources(configurationContext);
		PrepareApiScopes(configurationContext);
		PrepareApiResources(configurationContext);
	}
	public static void PrepareData(this IHost builder)
	{
		var userManager = builder.Services.GetService<UserManager<ApplicationUser>>();
		var roleManager = builder.Services.GetService<RoleManager<IdentityRole>>();
		if (userManager is null || roleManager is null) return;

		#region Roles Seed

		var adminRole = roleManager.FindByNameAsync("admin").Result;
		if (adminRole is null)
			roleManager.CreateAsync(new IdentityRole
			{
				Name = "admin"
			}).Wait();

		var sellerRole = roleManager.FindByNameAsync("member").Result;
		if (sellerRole is null)
			roleManager.CreateAsync(new IdentityRole
			{
				Name = "member"
			}).Wait();

		#endregion

		#region Account Seed

		#region Admin

		var adminAccount = userManager.FindByNameAsync("admin").Result;

		if (adminAccount is null) { }
		userManager.CreateAsync(new ApplicationUser
		{
			UserName = "admin",
			Email = "cemalnadirbusiness@gmail.com",
			Gender = Gender.Male,
			Name = "Cemal Nadir",
			PhoneNumber = "+905397207091",
			SurName = "Gorgorgil",
			BirthDate = new DateTime(1999, 8, 23, 9, 49, 0,DateTimeKind.Utc),
			EmailConfirmed = true,

		}, "cMYZ9ATA7P__").Wait();

		if (!userManager.IsInRoleAsync(userManager.FindByNameAsync("admin").Result!,
				roleManager.FindByNameAsync("admin").Result?.Name ?? "").Result)
			userManager.AddToRoleAsync(userManager.FindByNameAsync("admin").Result!,
				roleManager.FindByNameAsync("admin").Result?.Name ?? "").Wait();

		#endregion

		#endregion
	}

	#region Config Helpers

	private static void PrepareClients(ConfigurationDbContext context)
	{
		if (!context.Clients.Any())
		{
			context.AddClients();
		}

		if (MainHelper.IsEqual(context.Clients.Select(x => new { x.ClientId, x.AllowedScopes }).ToList(), Config.Clients.Select(x => new { x.ClientId, x.AllowedScopes }).ToList())) return;

		context.Clients.RemoveRange(context.Clients.ToList());
		context.SaveChanges();
		context.AddClients();
	}
	private static void PrepareApiResources(ConfigurationDbContext context)
	{
		if (!context.ApiResources.Any())
		{
			context.AddApiResources();
		}

		if (MainHelper.IsEqual(context.ApiResources.Select(x => x.Name).ToList(), Config.ApiResources.Select(x => x.Name).ToList())) return;

		context.ApiResources.RemoveRange(context.ApiResources.ToList());
		context.SaveChanges();
		context.AddApiResources();
	}
	private static void PrepareApiScopes(ConfigurationDbContext context)
	{
		if (!context.ApiScopes.Any())
		{
			context.AddApiScopes();
		}

		if (MainHelper.IsEqual(context.ApiScopes.Select(x => x.Name).ToList(), Config.ApiScopes.Select(x => x.Name))) return;

		context.ApiScopes.RemoveRange(context.ApiScopes.ToList());
		context.SaveChanges();
		context.AddApiScopes();
	}
	private static void PrepareIdentityResources(ConfigurationDbContext context)
	{
		if (!context.IdentityResources.Any())
		{
			context.AddIdentityResources();
		}

		if (MainHelper.IsEqual(context.IdentityResources.Select(x => x.Name).ToList(), Config.IdentityResources.Select(x => x.Name).ToList())) return;

		context.IdentityResources.RemoveRange(context.IdentityResources.ToList());
		context.SaveChanges();
		context.AddIdentityResources();
	}

	#endregion
}