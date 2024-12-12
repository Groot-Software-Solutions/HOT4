using System.ComponentModel;

namespace Hot4.Core.DataViewModels
{
    public class BankPaymentModel
    {
        [DisplayName("Payment Id")]
        public long PaymentID { get; set; }
        [DisplayName("Account")]
        public long AccountID { get; set; }
        [DisplayName("Type")]
        public byte PaymentTypeID { get; set; }
        public string PaymentType { get; set; }
        [DisplayName("Source")]
        public byte PaymentSourceID { get; set; }
        public string PaymentSource { get; set; }
        public decimal Amount { get; set; }
        [DisplayName("Date")]
        public DateTime PaymentDate { get; set; }
        public string Reference { get; set; }
        [DisplayName("Last User")]
        public string LastUser { get; set; }
    }
}
