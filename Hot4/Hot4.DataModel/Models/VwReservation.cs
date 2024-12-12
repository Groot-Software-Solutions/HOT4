namespace Hot4.DataModel.Models;

public partial class VwReservation
{
    public required string ReservationState { get; set; }

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
}
