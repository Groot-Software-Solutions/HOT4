namespace Hot4.ViewModel
{
    public class BankTrxBatchToDo
    {
        public long BankTrxBatchId { get; set; }
        public byte BankId { get; set; }
        public DateTime BatchDate { get; set; }
        public required string BatchReference { get; set; }
        public required string LastUser { get; set; }
    }
}
