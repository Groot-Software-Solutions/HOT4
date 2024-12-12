using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblReservation
{
    [Key]
    public long ReservationId { get; set; }

    public long AccessId { get; set; }

    public long RechargeId { get; set; }

    public byte BrandId { get; set; }

    public string? ProductCode { get; set; }

    public string? NotificationNumber { get; set; }

    public required string TargetNumber { get; set; }

    public DateTime InsertDate { get; set; }

    public byte StateId { get; set; }

    public decimal Amount { get; set; }

    public int Currency { get; set; }

    public string? ConfirmationData { get; set; }

    public virtual Access Access { get; set; }

    public virtual TblBrand Brand { get; set; }

    public virtual TblRecharge Recharge { get; set; }

    public virtual TblReservationState State { get; set; }
}
