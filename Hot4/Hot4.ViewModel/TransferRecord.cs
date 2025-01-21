namespace Hot4.ViewModel
{
    public class TransferRecord
    {
        public long TransferId { get; set; }
        public byte ChannelId { get; set; }
        public long PaymentIdFrom { get; set; }
        public long PaymentIdTo { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public long Smsid { get; set; }
    }
}
