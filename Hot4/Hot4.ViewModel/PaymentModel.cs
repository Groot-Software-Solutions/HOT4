namespace Hot4.ViewModel
{
    public class PaymentModel
    {
        public long PaymentId { get; set; }
        public long AccountId { get; set; }
        public byte PaymentTypeId { get; set; }
        public string PaymentType { get; set; }
        public byte PaymentSourceId { get; set; }
        public string PaymentSource { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Reference { get; set; }
        public string LastUser { get; set; }
    }
}
