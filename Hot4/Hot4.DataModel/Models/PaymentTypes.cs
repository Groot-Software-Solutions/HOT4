using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class PaymentTypes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte PaymentTypeId { get; set; }

    public required string PaymentType { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
