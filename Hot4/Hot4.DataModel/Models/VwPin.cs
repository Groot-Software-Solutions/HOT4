namespace Hot4.DataModel.Models;

public partial class VwPin
{
    public long PinId { get; set; }

    public long PinBatchId { get; set; }

    public required string PinBatch { get; set; }

    public DateTime BatchDate { get; set; }

    public byte PinStateId { get; set; }

    public required string PinState { get; set; }

    public byte BrandId { get; set; }

    public byte NetworkId { get; set; }

    public required string Network { get; set; }

    public required string Prefix { get; set; }

    public required string BrandName { get; set; }

    public required string BrandSuffix { get; set; }

    public required string Pin { get; set; }

    public required string PinRef { get; set; }

    public decimal PinValue { get; set; }

    public DateTime PinExpiry { get; set; }

    public byte PinBatchTypeId { get; set; }

    public required string PinBatchType { get; set; }

    public required string PinNumber { get; set; }
}
