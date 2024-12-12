using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPinBatch
{
    [Key]
    public long PinBatchId { get; set; }

    public required string PinBatch { get; set; }

    public DateTime BatchDate { get; set; }

    public byte PinBatchTypeId { get; set; }

    public virtual TblPinBatchType PinBatchType { get; set; }

    public virtual ICollection<TblPin> TblPins { get; set; } = new List<TblPin>();
}
