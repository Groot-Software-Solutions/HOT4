namespace Hot4.DataModel.Models;

public partial class Bundle
{
    public int BundleId { get; set; }

    public int BrandId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }


    public int Amount { get; set; }

    public required string ProductCode { get; set; }

    public int? ValidityPeriod { get; set; }

    public bool Enabled { get; set; }
}
