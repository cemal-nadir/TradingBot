using CNG.Abstractions.Signatures;
using MongoDB.Bson;
using TradingBot.Backend.Gateway.API.Dtos.Enums;

namespace TradingBot.Backend.Gateway.API.Dtos.Requests.Users
{
	public class TradingAccountDto:IDto
	{
		public string? UserId { get; set; }
		public string? Name { get; set; }
		public string? ApiKey { get; set; }
		public string? SecretKey { get; set; }
		public bool IsActive { get; set; }
		public TradingPlatform Platform { get; set; }

        public BalanceDetailDto? BalanceSettings { get; set; }
        public List<IndicatorDto>? Indicators { get; set; }

        public class BalanceDetailDto
        {
            public decimal CurrentBalance { get; set; }
            public decimal MinimumBalance { get; set; }
            public BudgetPlanDto? Plan { get; set; }
            public class BudgetPlanDto
            {
                public decimal AdjustBalancePercentage { get; set; }
                public decimal CurrentAdjustedBalance { get; set; }
                public int AdjustFrequencyDay { get; set; }
                public DateTime? LastAdjust { get; set; }
                public LossBasedPlanDto? LossBased { get; set; }
                public class LossBasedPlanDto
                {
                    public decimal LossAmountPercentage { get; set; }
                    public decimal? StopLossPercentage { get; set; }
                }
            }
        }
        public class IndicatorDto
        {
            private string? _id;

            public string? Id
            {
                get => _id;
                set => _id = string.IsNullOrEmpty(value) ? ObjectId.GenerateNewId().ToString() : value;
            }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
