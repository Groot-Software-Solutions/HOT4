namespace Hot4.DataModel.Models;

public partial class VwPaymentDetail
{
    public required string PaymentType { get; set; }

    public required string PaymentSource { get; set; }

    public long PaymentId { get; set; }

    public long AccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public required string Reference { get; set; }

    public long Expr1 { get; set; }

    public required string AccessCode { get; set; }

    public byte ChannelId { get; set; }

    public bool? Deleted { get; set; }
}
