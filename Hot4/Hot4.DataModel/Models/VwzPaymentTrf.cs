namespace Hot4.DataModel.Models;

public partial class VwzPaymentTrf
{
    public long AccountId { get; set; }

    public required string AccountName { get; set; }

    public long PaymentId { get; set; }

    public byte PaymentTypeId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public required string Reference { get; set; }

    public string? From { get; set; }

    public string? To { get; set; }

    public long? AccountTo { get; set; }

    public string? AccountNameTo { get; set; }
}
