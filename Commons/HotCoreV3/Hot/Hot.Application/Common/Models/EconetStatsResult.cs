namespace Hot.Application.Common.Models
{
    public class EconetStatsResult
    {
        public string Platform { get; set; } = string.Empty;
        public int TranCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
