using CNG.Core.Exceptions;
using CNG.Extensions;
using TradingBot.Backend.Libraries.ApiCore.Installers;
using TradingBot.Backend.Libraries.ApiCore.Middlewares;
using TradingBot.Backend.Libraries.Application;
using TradingBot.Backend.Services.Binance.API.Caps;

namespace TradingBot.Backend.Services.Binance.API.Extensions
{
	public static class InstallerExtensions
	{
		public static void InstallAllServices(this IServiceCollection services)
		{
			services.AddControllers();
			services.AddCors(options => options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
			services.AddConsoleLogService();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			var env = new EnvironmentModel
			{
                MongoDb = new MongoDbModel(Environment.GetEnvironmentVariable("MONGODB_CONNECTION") ?? ""
                ),
                RabbitMq = new RabbitMqModel(
					host: Environment.GetEnvironmentVariable(variable: "RABBITMQ_HOST") ?? "",
					userName: Environment.GetEnvironmentVariable(variable: "RABBITMQ_USER_NAME") ?? "",
					password: Environment.GetEnvironmentVariable(variable: "RABBITMQ_PASSWORD") ?? "",
					port: Environment.GetEnvironmentVariable(variable: "RABBITMQ_PORT").ToInt()
				),
				Project = new ProjectModel(projectName: Environment.GetEnvironmentVariable(variable: "PROJECT_NAME") ?? "",
					groupName: Environment.GetEnvironmentVariable(variable: "GROUP_NAME") ?? ""),
				Identity = new Libraries.Application.IdentityModel(
					identityUrl: Environment.GetEnvironmentVariable(variable: "IDENTITY_URL") ?? "",
					identityResourceName: Environment.GetEnvironmentVariable(variable: "IDENTITY_RESOURCE_NAME") ?? "")
			};

			services.AddSingleton(env);
			services.AddSwaggerServiceWithBearer(env.Project.ProjectName ?? throw new NotFoundException("Project name not found"));

			var installers = typeof(IServiceInstaller).Assembly.ExportedTypes
				.Where(predicate: x => typeof(IServiceInstaller).IsAssignableFrom(c: x) && x is { IsInterface: false, IsAbstract: false })
				.OrderBy(keySelector: x => x.Name)
				.Select(selector: Activator.CreateInstance)
				.Cast<IServiceInstaller>().ToList();
			installers.AddRange(typeof(InstallerExtensions).Assembly.ExportedTypes
				.Where(predicate: x => typeof(IServiceInstaller).IsAssignableFrom(c: x) && x is { IsInterface: false, IsAbstract: false })
				.OrderBy(keySelector: x => x.Name)
				.Select(selector: Activator.CreateInstance)
				.Cast<IServiceInstaller>().ToList());
			installers.ForEach(action: x => x.InstallServices(services: services, env: env));
			services.AddScoped<PullSubscribeService>();


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
			installers.AddRange(typeof(InstallerExtensions).Assembly.ExportedTypes
				.Where(predicate: x => typeof(IConfigureInstaller).IsAssignableFrom(c: x) && x is { IsInterface: false, IsAbstract: false })
				.OrderBy(keySelector: x => x.Name)
				.Select(selector: Activator.CreateInstance)
				.Cast<IConfigureInstaller>().ToList());
			installers.ForEach(x => x.InstallConfigures(app));
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

		}
	}
}
