using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPriority
{
    [Key]
    public byte PriorityId { get; set; }

    public required string Priority { get; set; }

    public virtual ICollection<TblSms> TblSms { get; set; } = new List<TblSms>();
}
