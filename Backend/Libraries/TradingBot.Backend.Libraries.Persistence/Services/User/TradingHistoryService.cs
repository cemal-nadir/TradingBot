using AutoMapper;
using Microsoft.AspNetCore.Http;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Repositories.User;
using TradingBot.Backend.Libraries.Application.Services.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;
using TradingBot.Backend.Libraries.Domain.Enums;

namespace TradingBot.Backend.Libraries.Persistence.Services.User
{
	public class TradingHistoryService : ServiceRepository<TradingHistory, TradingHistoryDto, string, ITradingHistoryRepository>, ITradingHistoryService
	{
		private readonly ITradingHistoryRepository _repository;
		private readonly IMapper _mapper;
		public TradingHistoryService(ITradingHistoryRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<TradingHistoryDto?> GetLastOrderForSymbolAsync(string symbol, string tradingAccountId, OrderType orderType,
				CancellationToken cancellationToken = default)
		{
			var model= await _repository.GetLastOrderForSymbolAsync(symbol, tradingAccountId, orderType, cancellationToken);
			return _mapper.Map<TradingHistoryDto>(model);
		}
	}
}
