using AutoMapper;
using Microsoft.AspNetCore.Http;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Repositories.User;
using TradingBot.Backend.Libraries.Application.Services.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Libraries.Persistence.Services.User
{
	public class TradingHistoryService:ServiceRepository<TradingHistory,TradingHistoryDto,string,ITradingHistoryRepository>,ITradingHistoryService
	{
		public TradingHistoryService(ITradingHistoryRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
		{
		}
	}
}
