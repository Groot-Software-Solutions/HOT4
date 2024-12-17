using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class PinBatches
{
    [Key]
    public long PinBatchId { get; set; }

    public required string PinBatch { get; set; }

    public DateTime BatchDate { get; set; }

    public byte PinBatchTypeId { get; set; }

    public virtual PinBatchTypes PinBatchType { get; set; }

    public virtual ICollection<Pins> Pins { get; set; } = new List<Pins>();
}
