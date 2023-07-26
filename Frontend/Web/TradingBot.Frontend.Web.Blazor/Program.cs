using TradingBot.Frontend.Web.Blazor.Installers;

var builder = WebApplication.CreateBuilder(args);

builder.InstallAllServices();

var app = builder.Build();

app.UseAllConfigurations();

app.Run();
