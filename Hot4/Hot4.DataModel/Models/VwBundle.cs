namespace Hot4.DataModel.Models;

public partial class VwBundle
{
    public int BundleId { get; set; }

    public int BrandId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public int Amount { get; set; }

    public required string ProductCode { get; set; }

    public int? ValidityPeriod { get; set; }

    public bool Enabled { get; set; }

    public required string Network { get; set; }
}
