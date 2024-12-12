namespace Hot4.DataModel.Models;

public partial class VwAccountOld
{
    public long AccountId { get; set; }

    public int ProfileId { get; set; }

    public required string ProfileName { get; set; }

    public required string AccountName { get; set; }

    public required string NationalId { get; set; }

    public required string Email { get; set; }

    public required string ReferredBy { get; set; }

    public decimal? Balance { get; set; }

    public decimal? SaleValue { get; set; }

    public decimal? Zesabalance { get; set; }
}
