using Hot.Domain.Enums;

namespace Hot.Domain.Entities;
public class Reservation
{
    public long ReservationId { get; set; }

    public long AccessID { get; set; }

    public long RechargeID { get; set; }

    public Brands BrandId { get; set; }

    public string ProductCode { get; set; }

    public string NotificationNumber { get; set; }

    public string TargetNumber { get; set; }

    public DateTime InsertDate { get; set; }

    public byte StateId { get; set; }

    public decimal Amount { get; set; }

    public Currency Currency { get; set; }
    public string? ConfirmationData { get; set; } 

}
