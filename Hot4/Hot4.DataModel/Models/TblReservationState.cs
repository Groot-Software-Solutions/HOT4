using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblReservationState
{
    [Key]
    public byte ReservationStateId { get; set; }

    public required string ReservationState { get; set; }

    public virtual ICollection<TblReservation> TblReservations { get; set; } = new List<TblReservation>();
}
