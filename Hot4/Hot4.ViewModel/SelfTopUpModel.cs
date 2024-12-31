namespace Hot4.ViewModel
{
    public class SelfTopUpModel
    {
        public long SelfTopUpId { get; set; }
        public long AccessId { get; set; }
        public long? BankTrxId { get; set; }
        public long? RechargeId { get; set; }
        public byte BrandId { get; set; }
        public string ProductCode { get; set; }
        public string NotificationNumber { get; set; }
        public string BillerNumber { get; set; }
        public DateTime InsertDate { get; set; }
        public string TargetNumber { get; set; }
        public byte StateId { get; set; }
        public decimal Amount { get; set; }
        public int Currency { get; set; }
        public string AccessCode { get; set; }
        public string BrandName { get; set; }
        public string SelfTopUpStateName { get; set; }
    }
}
