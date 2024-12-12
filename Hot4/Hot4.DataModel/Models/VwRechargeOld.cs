namespace Hot4.DataModel.Models;

public partial class VwRechargeOld
{
    public long RechargeId { get; set; }

    public byte StateId { get; set; }

    public required string State { get; set; }

    public long AccessId { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public required string Mobile { get; set; }

    public byte BrandId { get; set; }

    public byte NetworkId { get; set; }

    public required string Network { get; set; }

    public required string Prefix { get; set; }

    public required string BrandName { get; set; }

    public required string BrandSuffix { get; set; }

    public DateTime RechargeDate { get; set; }
}
