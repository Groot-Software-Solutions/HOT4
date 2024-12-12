namespace Hot4.DataModel.Models;

public partial class VwPlatformDetail
{
    public long RechargeId { get; set; }

    public byte StateId { get; set; }

    public required string State { get; set; }

    public long AccessId { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public required string Mobile { get; set; }

    public byte BrandId { get; set; }

    public DateTime RechargeDate { get; set; }

    public bool DebitCredit { get; set; }

    public required string ReturnCode { get; set; }

    public required string Narrative { get; set; }

    public decimal InitialBalance { get; set; }

    public decimal FinalBalance { get; set; }

    public required string Reference { get; set; }
}
