
namespace Hot.Application.Common.Models
{
    public class LimitModel
    {
        public decimal RemainingLimit { get; set; }
        public decimal RemainingDailyLimit { get; set; }
        public decimal RemainingMonthlyLimit { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal MonthlyLimit { get; set; }
        public decimal SalesToday { get; set; }
        public decimal SalesMonthly { get; set; }
        public byte LimitTypeId { get; set; }
        public int NetworkId { get; set; }
    }
}
