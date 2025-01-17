namespace Hot4.ViewModel
{
    public class PinBatchToDo
    {
        public long PinBatchId { get; set; }
        public required string PinBatch { get; set; }
        public DateTime BatchDate { get; set; }
        public byte PinBatchTypeId { get; set; }
    }
}
