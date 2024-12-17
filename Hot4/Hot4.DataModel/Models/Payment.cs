using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Payment
{
    [Key]
    public long PaymentId { get; set; }

    public long AccountId { get; set; }

    public byte PaymentTypeId { get; set; }

    public byte PaymentSourceId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public required string Reference { get; set; }

    public required string LastUser { get; set; }

    public virtual Account Account { get; set; }

    public virtual PaymentSources PaymentSource { get; set; }

    public virtual PaymentTypes PaymentType { get; set; }

    public virtual ICollection<BankTrx> BankTrxes { get; set; } = new List<BankTrx>();

    public virtual ICollection<Transfer> TransferPaymentIdFromNavigations { get; set; } = new List<Transfer>();

    public virtual ICollection<Transfer> TransferPaymentIdToNavigations { get; set; } = new List<Transfer>();
}
