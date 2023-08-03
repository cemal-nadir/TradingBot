using CNG.Abstractions.Signatures;

namespace TradingBot.Backend.Libraries.Application.Dtos.User
{
	public class TradingAccountsDto :TradingAccountDto, IListDto<string?>
	{
		public string? Id { get; set; }
		public string? UserMail { get; set; }
		public string? UserName { get; set; }
	}
}
