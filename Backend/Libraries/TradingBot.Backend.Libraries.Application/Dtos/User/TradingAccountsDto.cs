using CNG.Abstractions.Signatures;

namespace TradingBot.Backend.Libraries.Application.Dtos.User
{
	public class TradingAccountsDto : IListDto<string?>
	{
		public string? Id { get; set; }
	}
}
