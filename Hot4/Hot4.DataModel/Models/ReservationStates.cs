using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class ReservationStates
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte ReservationStateId { get; set; }

    public required string ReservationState { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
