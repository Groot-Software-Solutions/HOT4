using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblAccount
{
    [Key]
    public long AccountId { get; set; }

    public int ProfileId { get; set; }

    public required string AccountName { get; set; }

    public required string NationalId { get; set; }

    public required string Email { get; set; }

    public required string ReferredBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public virtual TblProfile Profile { get; set; }

    public virtual ICollection<Access> TblAccesses { get; set; } = new List<Access>();

    public virtual TblAddress? TblAddress { get; set; }

    public virtual ICollection<TblPayment> TblPayments { get; set; } = new List<TblPayment>();

    public virtual ICollection<TblSubscriber> TblSubscribers { get; set; } = new List<TblSubscriber>();
}
