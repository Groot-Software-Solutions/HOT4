using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblBankTrx
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

    public virtual TblBankTrxBatch BankTrxBatch { get; set; }

    public virtual TblBankTrxState BankTrxState { get; set; }

    public virtual TblBankTrxType BankTrxType { get; set; }

    public virtual TblPayment? Payment { get; set; }

    public virtual ICollection<TblSelfTopUp> TblSelfTopUps { get; set; } = new List<TblSelfTopUp>();
}
