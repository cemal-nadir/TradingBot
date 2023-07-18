
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
			CreateMap<Indicator, IndicatorDto>().ReverseMap();
		}
	}
}
