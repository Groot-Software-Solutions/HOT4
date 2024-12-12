using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class BankTrxTypes
{
    [Key]
    public byte BankTrxTypeId { get; set; }

    public required string BankTrxType { get; set; }

    public virtual ICollection<BankTrx> BankTrxes { get; set; } = new List<BankTrx>();
}
