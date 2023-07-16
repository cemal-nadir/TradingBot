using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TradingBot.Backend.Libraries.Application;

namespace TradingBot.Backend.Libraries.ApiCore.Installers.Services
{
    public class Identity:IServiceInstaller,IConfigureInstaller
    {
        public void InstallServices(IServiceCollection services, EnvironmentModel env)
        {
            if (env.Identity is null) return;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = env.Identity?.IdentityUrl;
                options.Audience = env.Identity?.IdentityResourceName;
                options.RequireHttpsMetadata = false;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Full", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", $"{env.Identity?.IdentityResourceName}.full");
                });
                options.AddPolicy("FullOrRead", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(c =>
                                   c.Type == "scope" && c.Value == $"{env.Identity?.IdentityResourceName}.full") ||
                               context.User.HasClaim(c =>
                                   c.Type == "scope" && c.Value == $"{env.Identity?.IdentityResourceName}.read");
                    });
                });
                options.AddPolicy("FullOrWrite", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context =>
                    {
                        return context.User.HasClaim(c =>
                                   c.Type == "scope" && c.Value == $"{env.Identity?.IdentityResourceName}.full") ||
                               context.User.HasClaim(c =>
                                   c.Type == "scope" && c.Value == $"{env.Identity?.IdentityResourceName}.write");
                    });
                });
                options.AddPolicy("Write", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", $"{env.Identity?.IdentityResourceName}.write");
                });
                options.AddPolicy("Read", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", $"{env.Identity?.IdentityResourceName}.read");
                });
            });
        }

        public void InstallConfigures(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
