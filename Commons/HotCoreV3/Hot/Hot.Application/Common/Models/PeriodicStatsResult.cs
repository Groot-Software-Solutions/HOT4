namespace Hot.Application.Common.Models
{
    public class PeriodicStatsResult
    {
        public string Network { get; set; } = string.Empty;
        public int Period { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }

}
