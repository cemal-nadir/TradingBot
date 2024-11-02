
using AutoMapper;
using TradingBot.Backend.Libraries.Application.Dtos.User;
using TradingBot.Backend.Libraries.Domain.Entities.User;

namespace TradingBot.Backend.Libraries.Application.Mappings
{
	public class AllProfiles:Profile
	{
		public AllProfiles()
		{
			CreateMap<TradingAccount, TradingAccountDto>().ReverseMap();
			CreateMap<TradingAccount, TradingAccountsDto>();
			CreateMap<TradingHistory, TradingHistoryDto>().ReverseMap();
			CreateMap<TradingHistory, TradingHistoriesDto>();
			CreateMap<TradingAccount.Indicator, TradingAccountDto.IndicatorDto>().ReverseMap();
			CreateMap<TradingAccount.BalanceDetail, TradingAccountDto.BalanceDetailDto>().ReverseMap();
            CreateMap<TradingAccount.BalanceDetail.BudgetPlan, TradingAccountDto.BalanceDetailDto.BudgetPlanDto>().ReverseMap();
            CreateMap<TradingAccount.BalanceDetail.BudgetPlan.LossBasedPlan, TradingAccountDto.BalanceDetailDto.BudgetPlanDto.LossBasedPlanDto>().ReverseMap();


        }
    }
}
