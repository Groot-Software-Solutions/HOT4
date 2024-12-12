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

    public virtual TblAccount Account { get; set; }

    public virtual TblChannel Channel { get; set; }

    public virtual ICollection<TblRecharge> TblRecharges { get; set; } = new List<TblRecharge>();

    public virtual ICollection<TblReservation> TblReservations { get; set; } = new List<TblReservation>();

    public virtual ICollection<TblSelfTopUp> TblSelfTopUps { get; set; } = new List<TblSelfTopUp>();
}
