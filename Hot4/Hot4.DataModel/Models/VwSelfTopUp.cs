namespace Hot4.DataModel.Models;

public partial class VwSelfTopUp
{
    public long SelfTopUpId { get; set; }

    public long AccessId { get; set; }

    public long BankTrxId { get; set; }

    public long? RechargeId { get; set; }

    public byte BrandId { get; set; }

    public string? ProductCode { get; set; }

    public string? NotificationNumber { get; set; }

    public DateTime InsertDate { get; set; }

    public byte StateId { get; set; }

    public decimal Amount { get; set; }

    public required string TargetNumber { get; set; }

    public required string AccessCode { get; set; }

    public required string BrandName { get; set; }

    public required string SelfTopUpStateName { get; set; }

    public required string Currency { get; set; }

    public required string BillerNumber { get; set; }
}
