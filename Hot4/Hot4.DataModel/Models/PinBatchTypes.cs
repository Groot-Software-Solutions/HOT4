using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class PinBatchTypes
{
    [Key]
    public byte PinBatchTypeId { get; set; }

    public required string PinBatchType { get; set; }

    public virtual ICollection<PinBatches> PinBatches { get; set; } = new List<PinBatches>();
}
