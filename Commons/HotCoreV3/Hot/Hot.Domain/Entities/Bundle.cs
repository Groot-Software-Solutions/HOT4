
namespace Hot.Domain.Entities;
public class Bundle
{
    public int BundleID { get; set; }

    public int BrandID { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Amount { get; set; }

    public string ProductCode { get; set; } = string.Empty;

    public int? ValidityPeriod { get; set; }

    public bool Enabled { get; set; }

    public string Network { get; set; } = String.Empty;

}

