using CNG.Abstractions.Signatures;
using TradingBot.Backend.Libraries.Domain.Enums;
using TradingBot.Backend.Libraries.Domain.Signatures;

namespace TradingBot.Backend.Libraries.Domain.Entities.User
{
	public class TradingAccount: UpdatedBase,IEntity<string>
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? ApiKey { get; set; }
		public string? SecretKey { get; set; }
		public bool IsActive { get; set; }
		public TradingPlatform Platform { get; set; }
	}

	public class OrderOptions
	{
		//İşlem Yapmak için hesapta bulunması gereken minimum usdt değeri eğer 0'sa işleme girmez
		public decimal MinimumBalance { get; set; }
		//İşlem Yaparken mevcut içeride bulunan usdt değerinin yüzde kaçı ile işlem yapılsın 0'sa tümüyle girer
		public decimal BalancePercentageForOrder { get; set; }

	}
}
