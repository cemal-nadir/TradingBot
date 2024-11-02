using DotNetCore.CAP;
using TradingBot.Backend.Libraries.Application.Services.Hangfire;
using TradingBot.Backend.Libraries.Application.Services.User;

namespace TradingBot.Backend.Libraries.Persistence.Services.Hangfire
{
    public class HangfireService : IHangfireService
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly ICapPublisher _capPublisher;
        private readonly ITradingAccountService _tradingAccountService;

        public HangfireService(ICapPublisher capPublisher, ITradingAccountService tradingAccountService)
        {
            _capPublisher = capPublisher;
            _tradingAccountService = tradingAccountService;
        }

        public async Task ReCalculateAdjustedBalances(CancellationToken cancellationToken = default)
        {
            await _tradingAccountService.UpdateAdjustedBalance(cancellationToken);
        }
    }
}
