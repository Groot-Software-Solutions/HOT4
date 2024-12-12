namespace Hot4.DataModel.Models;

public partial class VwAccountWeb
{
    public int ProfileId { get; set; }

    public required string AccountName { get; set; }

    public long AccountId { get; set; }

    public required string NationalId { get; set; }

    public required string Email { get; set; }

    public long? VatNumber { get; set; }

    public string? ContactNumber { get; set; }

    public string? ContactName { get; set; }

    public string? City { get; set; }

    public string? Address2 { get; set; }

    public string? Address1 { get; set; }

    public double? LatitudeY { get; set; }

    public double? LongitudeX { get; set; }

    public decimal? Balance { get; set; }

    public decimal? SaleValue { get; set; }

    public required string ProfileName { get; set; }
}
