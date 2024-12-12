namespace Hot4.DataModel.Models;

public partial class VwPinBatch
{
    public long PinBatchId { get; set; }

    public required string PinBatch { get; set; }

    public DateTime BatchDate { get; set; }

    public byte PinBatchTypeId { get; set; }

    public required string PinBatchType { get; set; }
}
