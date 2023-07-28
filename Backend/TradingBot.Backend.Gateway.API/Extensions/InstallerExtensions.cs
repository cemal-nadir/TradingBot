using System.Text;
using CNG.Core.Exceptions;
using CNG.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TradingBot.Backend.Gateway.API.Dtos;
using TradingBot.Backend.Gateway.API.Middlewares;

namespace TradingBot.Backend.Gateway.API.Extensions
{
	public static class InstallerExtensions
	{
		public static void InstallAllServices(this IServiceCollection services)
		{
			services.AddControllers();
			services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
			services.AddConsoleLogService();
			services.AddHttpContextAccessor();
			var env = new EnvironmentModel
			{
				MongoDb = new MongoDbModel(
					host: Environment.GetEnvironmentVariable(variable: "MONGODB_HOST") ?? "",
					userName: Environment.GetEnvironmentVariable(variable: "MONGODB_USER_NAME") ?? "",
					password: Environment.GetEnvironmentVariable(variable: "MONGODB_PASSWORD") ?? "",
					port: Environment.GetEnvironmentVariable(variable: "MONGODB_PORT").ToInt()
				),
				RabbitMq = new RabbitMqModel(
					host: Environment.GetEnvironmentVariable(variable: "RABBITMQ_HOST") ?? "",
					userName: Environment.GetEnvironmentVariable(variable: "RABBITMQ_USER_NAME") ?? "",
					password: Environment.GetEnvironmentVariable(variable: "RABBITMQ_PASSWORD") ?? "",
					port: Environment.GetEnvironmentVariable(variable: "RABBITMQ_PORT").ToInt()
				),
				MicroServices = new MicroServices()
				{
					Identity = Environment.GetEnvironmentVariable("SERVICE_IDENTITY"),
					User = Environment.GetEnvironmentVariable("SERVICE_USER"),
					Binance = Environment.GetEnvironmentVariable("SERVICE_BINANCE"),
				},
				ProjectSettings = new ProjectSettings()
				{
					GroupName = Environment.GetEnvironmentVariable("GROUP_NAME"),
					ProjectName = Environment.GetEnvironmentVariable("PROJECT_NAME")

				},
				Clients = new Clients()
				{
					
					Full = new ClientBase()
					{
						Id = Environment.GetEnvironmentVariable("API_CLIENTS_FULL_ID"),
						Secret = Environment.GetEnvironmentVariable("API_CLIENTS_FULL_SECRET"),
					},
					User = new ClientBase()
					{
						Id = Environment.GetEnvironmentVariable("API_CLIENTS_MEMBER_ID"),
						Secret = Environment.GetEnvironmentVariable("API_CLIENTS_MEMBER_SECRET"),
					},
				},
				Project = new ProjectModel(projectName: Environment.GetEnvironmentVariable(variable: "PROJECT_NAME") ?? "",
					groupName: Environment.GetEnvironmentVariable(variable: "GROUP_NAME") ?? ""),
			};

			services.AddSingleton(env);
			services.AddSwaggerService(env.Project?.ProjectName ?? throw new NotFoundException("Project name not found"));
			services.AddMemoryCache();

			var installers = typeof(IServiceInstaller).Assembly.ExportedTypes
				.Where(predicate: x => typeof(IServiceInstaller).IsAssignableFrom(c: x) && x is { IsInterface: false, IsAbstract: false })
				.OrderBy(keySelector: x => x.Name)
				.Select(selector: Activator.CreateInstance)
				.Cast<IServiceInstaller>().ToList();
		
			installers.ForEach(action: x => x.InstallServices(services: services, env: env));
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
			ServiceTool.Create(services);
		}

		public static void UseAllConfigurations(this IApplicationBuilder app)
		{

			var env = app.ApplicationServices.GetService<IWebHostEnvironment>();
			if (env != null && env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseMiddleware(typeof(ErrorHandlingMiddleware));
			app.UseRouting();
			app.UseCors();
			app.UseStaticFiles();
			app.UseSwaggerService();
			var installers = typeof(IConfigureInstaller).Assembly.ExportedTypes
				.Where(x => typeof(IConfigureInstaller).IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false })
				.OrderBy(x => x.Name)
				.Select(Activator.CreateInstance)
				.Cast<IConfigureInstaller>().ToList();
		
			installers.ForEach(x => x.InstallConfigures(app));
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

		}
	}
}
