namespace TradingBot.Backend.Libraries.Application
{
	public class EnvironmentModel
	{
		public DbModel? Database { get; set; }
		public MongoDbModel? MongoDb { get; set; }
		public RedisModel? Redis { get; set; }
		public RabbitMqModel? RabbitMq { get; set; }
		public FtpModel? Ftp { get; set; }
		public ProjectModel? Project { get; set; }
		public IdentityModel? Identity { get; set; }

	}

	public class IdentityModel
	{
		public IdentityModel(string identityUrl, string identityResourceName)
		{
			IdentityUrl = identityUrl;
			IdentityResourceName = identityResourceName;
		}
		public string? IdentityUrl { get; set; }
		public string? IdentityResourceName { get; set; }
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
	public class FtpModel
	{
		public FtpModel(string host, string userName, string password, int port)
		{
			Host = host;
			UserName = userName;
			Password = password;
			Port = port;
		}

		public string Host { get; }
		public string UserName { get; }
		public string Password { get; }
		public int Port { get; }
	}
	public class RedisModel
	{
		public RedisModel(string host, int port)
		{
			Host = host;
			Port = port;
			ConnectionString = string.Empty;
		}

		public RedisModel(string connectionString)
		{
			ConnectionString = connectionString;
			Host = string.Empty;
		}

		public string Host { get; }
		public int Port { get; }
		public string ConnectionString { get; }
	}
}
