using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class PaymentSources
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte PaymentSourceId { get; set; }

    public required string PaymentSource { get; set; }

    public int WalletTypeId { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
