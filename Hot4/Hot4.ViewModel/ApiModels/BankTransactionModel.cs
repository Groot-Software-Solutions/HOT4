namespace Hot4.ViewModel.ApiModels
{
    public class BankTransactionModel
    {
        public long BankTrxId { get; set; }
        public long BankTrxBatchId { get; set; }
        public byte BankTrxTypeId { get; set; }
        public string BankTrxType { get; set; }
        public byte BankTrxStateId { get; set; }
        public string BankTrxState { get; set; }
        public decimal Amount { get; set; }
        public DateTime TrxDate { get; set; }
        public string Identifier { get; set; }
        public string RefName { get; set; }
        public string Branch { get; set; }
        public string BankRef { get; set; }
        public decimal Balance { get; set; }
        public long? PaymentId { get; set; }
    }
}
