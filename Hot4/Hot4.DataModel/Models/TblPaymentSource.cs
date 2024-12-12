using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPaymentSource
{
    [Key]
    public byte PaymentSourceId { get; set; }

    public required string PaymentSource { get; set; }

    public int WalletTypeId { get; set; }

    public virtual ICollection<TblPayment> TblPayments { get; set; } = new List<TblPayment>();
}
