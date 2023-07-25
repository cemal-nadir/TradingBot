namespace TradingBot.Backend.Services.Identity.Api.Models;

public class EnvironmentModel
{
    public DbModel? DbModel { get; set; }
}
public class DbModel
{
    public DbModel(string host, string dbName, string userName, string password, int port)
    {
        Host = host;
        DbName = dbName;
        UserName = userName;
        Password = password;
        Port = port;
        ConnectionString = string.Empty;
    }

    public DbModel(string connectionString)
    {
        ConnectionString = connectionString;
        Host = string.Empty;
        DbName = string.Empty;
        UserName = string.Empty;
        Password = string.Empty;
    }

    public string Host { get; }
    public string DbName { get; }
    public string UserName { get; }
    public string Password { get; }
    public int Port { get; }
    public string ConnectionString { get; }
}