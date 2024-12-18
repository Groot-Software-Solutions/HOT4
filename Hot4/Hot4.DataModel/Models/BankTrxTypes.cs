using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class BankTrxTypes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte BankTrxTypeId { get; set; }
    public required string BankTrxType { get; set; }
    public virtual ICollection<BankTrx> BankTrxes { get; set; } = new List<BankTrx>();
}
