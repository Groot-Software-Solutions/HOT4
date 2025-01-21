namespace Hot4.ViewModel
{
    public class SubscriberRecord
    {
        public long SubscriberId { get; set; }
        public long AccountId { get; set; }
        public required string SubscriberName { get; set; }
        public required string SubscriberMobile { get; set; }
        public byte BrandId { get; set; }
        public bool Active { get; set; }
        public string? NotifyNumber { get; set; }
        public string? SubscriberGroup { get; set; }
        public int? DefaultProductId { get; set; }
        public decimal? DefaultAmount { get; set; }
        public byte? NetworkId { get; set; }
    }
}
