using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class PaymentTypes
{
    [Key]
    public byte PaymentTypeId { get; set; }

    public required string PaymentType { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
