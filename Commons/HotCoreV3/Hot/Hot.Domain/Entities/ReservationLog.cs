
namespace Hot.Domain.Entities;

public class ReservationLog
{
    public long ReservationLogId { get; set; }

    public long ReservationId { get; set; }

    public DateTime LogDate { get; set; }

    public byte OldStateId { get; set; }

    public byte NewStateId { get; set; }

    public string LastUser { get; set; }

}
