using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Priorities
{
    [Key]
    public byte PriorityId { get; set; }

    public required string Priority { get; set; }

    public virtual ICollection<Sms> Sms { get; set; } = new List<Sms>();
}
