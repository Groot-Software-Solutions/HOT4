using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Pins
{
    [Key]
    public long PinId { get; set; }

    public long PinBatchId { get; set; }

    public byte PinStateId { get; set; }

    public byte BrandId { get; set; }

    public required string Pin { get; set; }

    public required string PinRef { get; set; }

    public decimal PinValue { get; set; }

    public DateTime PinExpiry { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual PinBatches PinBatch { get; set; }

    public virtual PinStates PinState { get; set; }

    public virtual ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();
}
