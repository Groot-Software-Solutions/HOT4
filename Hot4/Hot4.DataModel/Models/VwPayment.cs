namespace Hot4.DataModel.Models;

public partial class VwPayment
{
    public long PaymentId { get; set; }

    public long AccountId { get; set; }

    public byte PaymentTypeId { get; set; }

    public required string PaymentType { get; set; }

    public byte PaymentSourceId { get; set; }

    public required string PaymentSource { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public required string Reference { get; set; }

    public required string LastUser { get; set; }
}
