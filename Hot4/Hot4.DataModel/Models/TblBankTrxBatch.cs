using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblBankTrxBatch
{
    [Key]
    public long BankTrxBatchId { get; set; }

    public byte BankId { get; set; }

    public DateTime BatchDate { get; set; }

    public required string BatchReference { get; set; }

    public required string LastUser { get; set; }

    public virtual TblBank Bank { get; set; }

    public virtual ICollection<TblBankTrx> TblBankTrxes { get; set; } = new List<TblBankTrx>();
}
