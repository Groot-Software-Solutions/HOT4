namespace Hot4.ViewModel
{
    public class BankvPaymentModel
    {
        public long? BankTrxId { get; set; }
        public Guid? VPaymentId { get; set; }
        public string? CheckUrl { get; set; }
        public string? ProcessUrl { get; set; }
        public string? ErrorMsg { get; set; }
        public string? VPaymentRef { get; set; }
    }
}
