namespace Hot4.DataModel.Models;

public partial class VwRechargeListDetail
{
    public long AccountId { get; set; }

    public required string AccessCode { get; set; }

    public long RechargeId { get; set; }

    public byte StateId { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public required string Mobile { get; set; }

    public byte BrandId { get; set; }

    public required string BrandName { get; set; }

    public DateTime RechargeDate { get; set; }

    public decimal? InitialBalance { get; set; }

    public decimal? FinalBalance { get; set; }

    public string? Narrative { get; set; }

    public string? Reference { get; set; }

    public string? ReturnCode { get; set; }

    public int? WalletTypeId { get; set; }
}
