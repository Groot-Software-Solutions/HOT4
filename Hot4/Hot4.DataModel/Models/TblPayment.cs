using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblPayment
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

    public virtual TblAccount Account { get; set; }

    public virtual TblPaymentSource PaymentSource { get; set; }

    public virtual TblPaymentType PaymentType { get; set; }

    public virtual ICollection<TblBankTrx> TblBankTrxes { get; set; } = new List<TblBankTrx>();

    public virtual ICollection<TblTransfer> TblTransferPaymentIdFromNavigations { get; set; } = new List<TblTransfer>();

    public virtual ICollection<TblTransfer> TblTransferPaymentIdToNavigations { get; set; } = new List<TblTransfer>();
}
