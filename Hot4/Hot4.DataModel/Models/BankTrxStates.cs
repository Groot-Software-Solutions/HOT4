using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class BankTrxStates
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte BankTrxStateId { get; set; }

    public required string BankTrxState { get; set; }

    public virtual ICollection<BankTrx> BankTrxes { get; set; } = new List<BankTrx>();
}
