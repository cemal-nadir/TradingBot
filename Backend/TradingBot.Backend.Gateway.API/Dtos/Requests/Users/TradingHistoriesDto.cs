using CNG.Abstractions.Signatures;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Users
{
	public class TradingHistoriesDto:TradingHistoryDto,IListDto<string?>
	{
		public string? Id { get; set; }
	}
}
