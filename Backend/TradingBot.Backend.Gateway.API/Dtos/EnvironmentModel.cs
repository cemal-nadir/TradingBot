namespace TradingBot.Backend.Gateway.API.Dtos
{
	public class EnvironmentModel
	{
		public MongoDbModel? MongoDb { get; set; }
		public RabbitMqModel? RabbitMq { get; set; }
		public MicroServices? MicroServices { get; set; }
		public ProjectSettings? ProjectSettings { get; set; }
		public Clients? Clients { get; set; }
		public ProjectModel? Project { get; set; }
	}
	public class ProjectModel
	{
		public ProjectModel(string projectName, string groupName)
		{
			ProjectName = projectName;
			GroupName = groupName;
		}
		public string? ProjectName { get; set; }
		public string? GroupName { get; set; }

	}
    public class MongoDbModel
    {
        public MongoDbModel(string host, string userName, string password, int port)
        {
            Host = host;
            UserName = userName;
            Password = password;
            Port = port;
            ConnectionString = string.Empty;
        }

        public MongoDbModel(string connectionString)
        {
            ConnectionString = connectionString;
            Host = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
        }

        public string Host { get; }
        public string UserName { get; }
        public string Password { get; }
        public int Port { get; }
        public string ConnectionString { get; }
    }
    public class RabbitMqModel
	{
		public RabbitMqModel(string host, string userName, string password, int port)
		{
			Host = host;
			UserName = userName;
			Password = password;
			Port = port;
			ConnectionString = string.Empty;
		}

		public RabbitMqModel(string connectionString)
		{
			ConnectionString = connectionString;
			Host = string.Empty;
			UserName = string.Empty;
			Password = string.Empty;
		}

		public string Host { get; }
		public string UserName { get; }
		public string Password { get; }
		public int Port { get; }

		public string ConnectionString { get; }
	}
	public class MicroServices
	{
		public string? User { get; set; }
		public string? Binance { get; set; }
		public string? Identity { get; set; }
	}

	public class ProjectSettings
	{
		public string? ProjectName { get; set; }
		public string? GroupName { get; set; }
	}
	public class Clients
	{
		public ClientBase? Full { get; set; }
		public ClientBase? User { get; set; }
	}

	public class ClientBase
	{
		public string? Id { get; set; }
		public string? Secret { get; set; }
	}
}
