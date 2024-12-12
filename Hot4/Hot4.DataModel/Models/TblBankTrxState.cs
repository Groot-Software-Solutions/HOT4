using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblBankTrxState
{
    [Key]
    public byte BankTrxStateId { get; set; }

    public required string BankTrxState { get; set; }

    public virtual ICollection<TblBankTrx> TblBankTrxes { get; set; } = new List<TblBankTrx>();
}
