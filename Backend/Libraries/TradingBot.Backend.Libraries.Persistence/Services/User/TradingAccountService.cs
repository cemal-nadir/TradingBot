using AutoMapper;
using CNG.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Application.Repositories.User;
using TradingBot.Backend.Libraries.Application.Services.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Libraries.Persistence.Services.User
{
	public class TradingAccountService:ServiceRepository<TradingAccount, TradingAccountDto,string,ITradingAccountRepository>,ITradingAccountService
	{
		private readonly ITradingAccountRepository _repository;
		private readonly IMapper _mapper;

        public TradingAccountService(ITradingAccountRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, httpContextAccessor)
		{
			_repository = repository;
			_mapper = mapper;
		}

		public async Task<TradingAccountsDto> GetByIndicatorId(string indicatorId,CancellationToken cancellationToken = default)
		{
			return _mapper.Map<TradingAccountsDto>(await _repository.GetByIndicatorId(indicatorId, cancellationToken) ??
			                                       throw new NotFoundException($"{nameof(TradingAccount.Indicator)} not found"));
		}

		public async Task<List<TradingAccountsDto>> GetAllAsync(CancellationToken cancellationToken = default)
		{
			return _mapper.Map<List<TradingAccountsDto>>(await _repository.GetAllAsync(null, cancellationToken));
		}
		public async Task<List<TradingAccountsDto>> GetAllByUserIdAsync(string userId, CancellationToken cancellationToken = default)
		{
			return _mapper.Map<List<TradingAccountsDto>>(await _repository.GetAllAsync(x=>x.UserId==userId, cancellationToken));
		}

        public async Task UpdateAdjustedBalance(CancellationToken cancellationToken = default)
        {
            await _repository.UpdateAdjustedBalances(cancellationToken);
        }
    }
}
