using Hot.Domain.Enums;

namespace Hot.Nyaradzo.API.Models
{
    public class PaymentProcessingRequest
    {
        public string PolicyNumber { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
        public decimal MonthlyPremium { get; set; }
        public int NumberOfMonthsPaid { get; set; }
        public DateTime Date { get; set; }
        public Currency currency { get; set; }
    }
}
