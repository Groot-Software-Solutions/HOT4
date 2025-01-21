using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class SelfTopUp
{
    [Key]
    public long SelfTopUpId { get; set; }

    public long AccessId { get; set; }

    public long? BankTrxId { get; set; }

    public long? RechargeId { get; set; }

    public byte BrandId { get; set; }

    public string? ProductCode { get; set; }

    public string? NotificationNumber { get; set; }

    public required string BillerNumber { get; set; }

    public required string TargetNumber { get; set; }

    public DateTime InsertDate { get; set; }

    public byte StateId { get; set; }

    public decimal Amount { get; set; }

    public int Currency { get; set; }

    public virtual Access Access { get; set; }

    public BankTrx? BankTrx { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual Recharge? Recharge { get; set; }
    public virtual SelfTopUpState SelfTopUpState { get; set; }
}
