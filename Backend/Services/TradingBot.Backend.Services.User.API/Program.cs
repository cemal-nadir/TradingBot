using TradingBot.Backend.Services.User.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InstallAllServices();

var app = builder.Build();

app.UseAllConfigurations();
app.Run();