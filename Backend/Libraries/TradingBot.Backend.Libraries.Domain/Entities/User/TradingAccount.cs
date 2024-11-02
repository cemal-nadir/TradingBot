using CNG.Abstractions.Signatures;
using MongoDB.Bson;
using TradingBot.Backend.Libraries.Domain.Enums;
using TradingBot.Backend.Libraries.Domain.Signatures;

namespace TradingBot.Backend.Libraries.Domain.Entities.User
{
    public class TradingAccount : UpdatedBase, IEntity<string>
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? ApiKey { get; set; }
        public string? SecretKey { get; set; }
        public bool IsActive { get; set; }
        public BalanceDetail? BalanceSettings { get; set; }
        public TradingPlatform Platform { get; set; }
        public List<Indicator>? Indicators { get; set; }
        public class BalanceDetail
        {
            public decimal CurrentBalance { get; set; }
            public decimal MinimumBalance { get; set; }
            public BudgetPlan? Plan { get; set; }
            public class BudgetPlan
            {
                public decimal AdjustBalancePercentage { get; set; }
                public decimal CurrentAdjustedBalance { get; set; }
                public int AdjustFrequencyDay { get; set; }
                public DateTime? LastAdjust { get; set; }
                public LossBasedPlan? LossBased { get; set; }
                public class LossBasedPlan
                {
                    public decimal LossAmountPercentage { get; set; }
                    public decimal? StopLossPercentage { get; set; }
                }
            }
        }
        public class Indicator : IEntity<string>
        {
            public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
            public string? Name { get; set; }
            public string? Description { get; set; }
            public bool IsActive { get; set; }
        }
    }

    
}
