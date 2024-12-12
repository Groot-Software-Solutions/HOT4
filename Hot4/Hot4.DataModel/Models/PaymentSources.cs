using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class PaymentSources
{
    [Key]
    public byte PaymentSourceId { get; set; }

    public required string PaymentSource { get; set; }

    public int WalletTypeId { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
