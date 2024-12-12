namespace Hot4.DataModel.Models;

public partial class VwProfileDiscount
{
    public int ProfileDiscountId { get; set; }

    public int ProfileId { get; set; }

    public byte BrandId { get; set; }

    public byte NetworkId { get; set; }

    public required string Network { get; set; }

    public required string Prefix { get; set; }

    public required string BrandName { get; set; }

    public required string BrandSuffix { get; set; }

    public decimal Discount { get; set; }
}
