using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class Priorities
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte PriorityId { get; set; }

    public required string Priority { get; set; }

    public virtual ICollection<Sms> Sms { get; set; } = new List<Sms>();
}
