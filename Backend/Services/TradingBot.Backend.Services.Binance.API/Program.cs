using TradingBot.Backend.Services.Binance.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallAllServices();

var app = builder.Build();

app.UseAllConfigurations();
app.Run();