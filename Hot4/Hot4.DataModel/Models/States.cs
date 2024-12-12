using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class States
{
    [Key]
    public byte StateId { get; set; }

    public required string State { get; set; }

    public virtual ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();

    public virtual ICollection<Sms> Sms { get; set; } = new List<Sms>();
}
