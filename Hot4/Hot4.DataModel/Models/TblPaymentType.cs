using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPaymentType
{
    [Key]
    public byte PaymentTypeId { get; set; }

    public required string PaymentType { get; set; }

    public virtual ICollection<TblPayment> TblPayments { get; set; } = new List<TblPayment>();
}
