using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class BankTrx
{
    [Key]
    public long BankTrxId { get; set; }
    public long BankTrxBatchId { get; set; }
    public byte BankTrxTypeId { get; set; }
    public byte BankTrxStateId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TrxDate { get; set; }
    public required string Identifier { get; set; }
    public required string RefName { get; set; }
    public required string Branch { get; set; }
    public required string BankRef { get; set; }
    public decimal Balance { get; set; }
    public long? PaymentId { get; set; }
    public virtual BankTrxBatch BankTrxBatch { get; set; }
    public virtual BankTrxStates BankTrxState { get; set; }
    public virtual BankTrxTypes BankTrxType { get; set; }
    public virtual Payment? Payment { get; set; }
    public virtual ICollection<SelfTopUp> SelfTopUps { get; set; } = new List<SelfTopUp>();
}
