namespace Hot4.ViewModel.ApiModels
{
    public class BankBatchModel
    {
        public long BankTrxBatchId { get; set; }
        public byte BankId { get; set; }
        public string BankName { get; set; }
        public DateTime BatchDate { get; set; }
        public string BatchReference { get; set; }
        public string LastUser { get; set; }
    }
}
