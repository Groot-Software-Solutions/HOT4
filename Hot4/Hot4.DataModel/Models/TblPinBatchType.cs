using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPinBatchType
{
    [Key]
    public byte PinBatchTypeId { get; set; }

    public required string PinBatchType { get; set; }

    public virtual ICollection<TblPinBatch> TblPinBatches { get; set; } = new List<TblPinBatch>();
}
