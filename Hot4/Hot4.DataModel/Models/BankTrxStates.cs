using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class BankTrxStates
{
    [Key]
    public byte BankTrxStateId { get; set; }

    public required string BankTrxState { get; set; }

    public virtual ICollection<BankTrx> BankTrxes { get; set; } = new List<BankTrx>();
}
