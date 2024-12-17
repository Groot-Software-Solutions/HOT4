using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class PinStates
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte PinStateId { get; set; }

    public required string PinState { get; set; }

    public virtual ICollection<Pins> Pins { get; set; } = new List<Pins>();
}
