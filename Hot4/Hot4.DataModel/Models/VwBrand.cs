namespace Hot4.DataModel.Models;

public partial class VwBrand
{
    public byte BrandId { get; set; }

    public byte NetworkId { get; set; }

    public required string Network { get; set; }

    public required string Prefix { get; set; }

    public required string BrandName { get; set; }

    public required string BrandSuffix { get; set; }

    public int? WalletTypeId { get; set; }
}
