using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblBankTrxType
{
    [Key]
    public byte BankTrxTypeId { get; set; }

    public required string BankTrxType { get; set; }

    public virtual ICollection<TblBankTrx> TblBankTrxes { get; set; } = new List<TblBankTrx>();
}
