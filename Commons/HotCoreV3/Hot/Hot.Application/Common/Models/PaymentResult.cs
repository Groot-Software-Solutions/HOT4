namespace Hot.Application.Common.Models
{
    public class PaymentResult
    {
        public long PaymentID { get; set; }
        public long AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
        public string PaymentSource { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string MyProperty { get; set; } = string.Empty;
    }
}
