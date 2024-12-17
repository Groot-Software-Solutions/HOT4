using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class ReservationLog
{
    [Key]
    public long ReservationLogId { get; set; }

    public long ReservationId { get; set; }

    public DateTime LogDate { get; set; }

    public byte OldStateId { get; set; }

    public byte NewStateId { get; set; }

    public required string LastUser { get; set; }

    public virtual ReservationStates NewState { get; set; }

    public virtual ReservationStates OldState { get; set; }

    public virtual Reservation Reservation { get; set; }
}
