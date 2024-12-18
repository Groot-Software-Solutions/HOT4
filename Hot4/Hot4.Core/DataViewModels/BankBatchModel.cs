namespace Hot4.Core.DataViewModels
{
    public class BankBatchModel
    {
        public long BankTrxBatchID { get; set; }
        public byte BankID { get; set; }
        public DateTime BatchDate { get; set; }
        public string BatchReference { get; set; }
        public string LastUser { get; set; }
    }
}
