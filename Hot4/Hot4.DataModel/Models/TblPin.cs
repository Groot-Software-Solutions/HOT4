using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPin
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

    public virtual TblBrand Brand { get; set; }

    public virtual TblPinBatch PinBatch { get; set; }

    public virtual TblPinState PinState { get; set; }

    public virtual ICollection<TblRecharge> Recharges { get; set; } = new List<TblRecharge>();
}
