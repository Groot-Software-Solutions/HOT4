namespace Hot4.Core.DataViewModels
{
    public class BankTransactionModel
    {
        public long BankTrxID { get; set; }
        public long BankTrxBatchID { get; set; }
        public byte BankTrxTypeID { get; set; }
        public string BankTrxType { get; set; }
        public byte BankTrxStateID { get; set; }
        public string BankTrxState { get; set; }
        public decimal Amount { get; set; }
        public DateTime TrxDate { get; set; }
        public string Identifier { get; set; }
        public string RefName { get; set; }
        public string Branch { get; set; }
        public string BankRef { get; set; }
        public decimal Balance { get; set; }
        public long? PaymentID { get; set; }
        public byte StockAccountID { get; set; }
    }
}
