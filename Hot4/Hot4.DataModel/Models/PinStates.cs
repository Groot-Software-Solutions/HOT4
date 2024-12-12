using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class PinStates
{
    [Key]
    public byte PinStateId { get; set; }

    public required string PinState { get; set; }

    public virtual ICollection<Pins> Pins { get; set; } = new List<Pins>();
}
