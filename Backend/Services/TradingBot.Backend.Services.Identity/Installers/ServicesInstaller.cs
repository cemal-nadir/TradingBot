using System.Reflection;
using IdentityModel;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TradingBot.Backend.Services.Identity.Api.CustomTokenProviders;
using TradingBot.Backend.Services.Identity.Api.Data;
using TradingBot.Backend.Services.Identity.Api.Middlewares;
using TradingBot.Backend.Services.Identity.Api.Models;
using TradingBot.Backend.Services.Identity.Api.Services;

namespace TradingBot.Backend.Services.Identity.Api.Installers;

public static class InstallerExtension
{
    public static void InstallAllServices(this IServiceCollection services)
    {
        services.AddLocalApiAuthentication();

        services.AddControllersWithViews();

        services.AddDbContext<AspNetIdentityDbContext>(options =>
            options.UseNpgsql(
                $"Server={Environment.GetEnvironmentVariable(variable: "DATABASE_HOST")};Port={Environment.GetEnvironmentVariable(variable: "DATABASE_PORT")};Database={Environment.GetEnvironmentVariable(variable: "DATABASE_NAME")};User Id={Environment.GetEnvironmentVariable(variable: "DATABASE_USER_NAME")};Password={Environment.GetEnvironmentVariable(variable: "DATABASE_PASSWORD")};"));
        services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            })
            .AddEntityFrameworkStores<AspNetIdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailconfirmation");
        services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        var migrationsAssembly = typeof(InstallerExtension).GetTypeInfo().Assembly.GetName().Name;
        var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;

            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseNpgsql(
                        $"Server={Environment.GetEnvironmentVariable(variable: "DATABASE_HOST")};Port={Environment.GetEnvironmentVariable(variable: "DATABASE_PORT")};Database={Environment.GetEnvironmentVariable(variable: "DATABASE_NAME")};User Id={Environment.GetEnvironmentVariable(variable: "DATABASE_USER_NAME")};Password={Environment.GetEnvironmentVariable(variable: "DATABASE_PASSWORD")};",
                        sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseNpgsql(
                        $"Server={Environment.GetEnvironmentVariable(variable: "DATABASE_HOST")};Port={Environment.GetEnvironmentVariable(variable: "DATABASE_PORT")};Database={Environment.GetEnvironmentVariable(variable: "DATABASE_NAME")};User Id={Environment.GetEnvironmentVariable(variable: "DATABASE_USER_NAME")};Password={Environment.GetEnvironmentVariable(variable: "DATABASE_PASSWORD")};",
                        sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            })
            .AddAspNetIdentity<ApplicationUser>();

        builder.AddResourceOwnerValidator<IdentityResourceOwnerPasswordValidator>();
        // not recommended for production - you need to store your key material somewhere secure
        builder.AddDeveloperSigningCredential();
        services.AddAuthentication();
  //      services.AddAuthentication().AddOpenIdConnect("oidc", options =>
  //      {
	 //       options.Scope.Add("roles");
	 //       options.ClaimActions.MapJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role, JwtClaimTypes.Role);
	 //       options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;
		//});

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(2));
        services.Configure<EmailConfirmationTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromDays(20));
		services.AddTransient<IProfileService, CustomProfileService>();
		services.AddScoped<IUserService, UserService>();


    }
    public static void UseAllConfigurations(this IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
        if (env != null && env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        app.UseMiddleware(typeof(ErrorHandlingMiddleware));
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors();

        app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
    }
}