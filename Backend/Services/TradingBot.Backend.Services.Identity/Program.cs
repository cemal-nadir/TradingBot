using TradingBot.Backend.Services.Identity.Api.Installers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.InstallAllServices();
var app= builder.Build();
app.UseAllConfigurations();
app.CreateDatabases();
app.PrepareConfig();
app.PrepareData();
app.Run();
