using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Account
{
    [Key]
    public long AccountId { get; set; }

    public int ProfileId { get; set; }

    public required string AccountName { get; set; }

    public required string NationalId { get; set; }

    public required string Email { get; set; }

    public required string ReferredBy { get; set; }

    public DateTime? InsertDate { get; set; }

    public virtual Profile Profile { get; set; }

    public virtual ICollection<Access> Accesses { get; set; } = new List<Access>();

    public virtual Address? Address { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Subscriber> Subscribers { get; set; } = new List<Subscriber>();
}
