namespace Hot4.DataModel.Models;

public partial class VwSubscriber
{
    public long SubscriberId { get; set; }

    public long AccountId { get; set; }

    public required string SubscriberName { get; set; }

    public required string SubscriberMobile { get; set; }

    public byte BrandId { get; set; }

    public byte NetworkId { get; set; }

    public required string Network { get; set; }

    public required string Prefix { get; set; }

    public required string BrandName { get; set; }

    public required string BrandSuffix { get; set; }

    public bool Active { get; set; }
}
