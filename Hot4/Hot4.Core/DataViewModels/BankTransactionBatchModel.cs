namespace Hot4.Core.DataViewModels
{
    public class BankTransactionBatchModel
    {
        public long BankTrxBatchID { get; set; }
        public long BankID { get; set; }
        public string Bank { get; set; }
        public DateTime BatchDate { get; set; }
        public string BatchReference { get; set; }
        public string LastUser { get; set; }
    }
}
