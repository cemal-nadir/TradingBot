namespace TradingBot.Backend.Gateway.API.Services.Abstract.Hangfire
{
	public interface IHangfireService
	{
		Task AdjustAllBalances(CancellationToken cancellationToken=default);
	}
}
