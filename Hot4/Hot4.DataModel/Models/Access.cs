using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Access
{
    [Key]
    public long AccessId { get; set; }

    public long AccountId { get; set; }

    public byte ChannelId { get; set; }

    public required string AccessCode { get; set; }

    public required string AccessPassword { get; set; }

    public bool? Deleted { get; set; }

    public string? PasswordHash { get; set; }

    public string? PasswordSalt { get; set; }

    public DateTime? InsertDate { get; set; }


    public virtual Account Account { get; set; }

    public virtual Channels Channel { get; set; }

    public virtual ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<SelfTopUp> SelfTopUps { get; set; } = new List<SelfTopUp>();
}
