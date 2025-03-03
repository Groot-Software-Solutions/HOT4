namespace Hot.Application.Common.Models
{
    public class StatementTransaction
    {
        public DateTime RechargeDate { get; set; }
        public int RechargeID { get; set; }
        public string TranType { get; set; } = string.Empty;
        public string AccessCode { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public decimal Cost { get; set; }
        public string Mobile { get; set; } = string.Empty;
    }
}
