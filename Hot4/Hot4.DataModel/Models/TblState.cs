using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblState
{
    [Key]
    public byte StateId { get; set; }

    public required string State { get; set; }

    public virtual ICollection<TblRecharge> TblRecharges { get; set; } = new List<TblRecharge>();

    public virtual ICollection<TblSms> TblSms { get; set; } = new List<TblSms>();
}
