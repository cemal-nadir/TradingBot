namespace TradingBot.Frontend.Libraries.Blazor.Models;

public class Env
{
	public Env(string identityUrl, string gatewayUrl, string clientId,
		string clientSecret)
	{
		IdentityUrl = identityUrl;
		GatewayUrl = gatewayUrl;
		Client = new Client()
		{
			Id = clientId,
			Secret = clientSecret
		};
	}

	public string IdentityUrl { get; }
    public string GatewayUrl { get; }
    public Client Client { get; }
}

public class Client
{
	public string? Id { get; set; }
	public string? Secret { get; set; }
}
