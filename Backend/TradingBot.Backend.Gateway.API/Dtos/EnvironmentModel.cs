namespace TradingBot.Backend.Gateway.API.Dtos
{
	public class EnvironmentModel
	{
		public MicroServices? MicroServices { get; set; }
		public ProjectSettings? ProjectSettings { get; set; }
		public Clients? Clients { get; set; }
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
