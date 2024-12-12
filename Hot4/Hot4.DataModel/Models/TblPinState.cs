using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPinState
{
    [Key]
    public byte PinStateId { get; set; }

    public required string PinState { get; set; }

    public virtual ICollection<TblPin> TblPins { get; set; } = new List<TblPin>();
}
