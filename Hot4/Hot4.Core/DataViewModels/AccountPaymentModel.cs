using System.ComponentModel;

namespace Hot4.Core.DataViewModels
{
    public class AccountPaymentModel
    {
        [DisplayName("Account Id")]
        public long AccountID { get; set; }

        [DisplayName("Payment Id")]
        public long PaymentID { get; set; }

        [DisplayName("Type")]
        public string PaymentType { get; set; }

        [DisplayName("Source")]
        public string PaymentSource { get; set; }

        [DisplayName("Stock Account")]
        public string StockAccountName { get; set; }

        public decimal Amount { get; set; }

        [DisplayName("Date")]
        public DateTime PaymentDate { get; set; }

        public string Reference { get; set; }

        [DisplayName("Last User")]
        public string LastUser { get; set; }
    }
}
