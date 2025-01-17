namespace Hot4.ViewModel
{
    public class BankTrxToDo
    {
        public long BankTrxId { get; set; }
        public long BankTrxBatchId { get; set; }
        public byte BankTrxTypeId { get; set; }
        public byte BankTrxStateId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TrxDate { get; set; }
        public required string Identifier { get; set; }
        public required string RefName { get; set; }
        public required string Branch { get; set; }
        public required string BankRef { get; set; }
        public decimal Balance { get; set; }
        public long? PaymentId { get; set; }
    }
}
