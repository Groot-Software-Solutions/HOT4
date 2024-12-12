using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class ReservationStates
{
    [Key]
    public byte ReservationStateId { get; set; }

    public required string ReservationState { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
