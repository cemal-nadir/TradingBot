namespace TradingBot.Backend.Libraries.Application.Services.Hangfire
{
	public interface IHangfireService
    {
        Task ReCalculateAdjustedBalances(CancellationToken cancellationToken = default);

    }
}
