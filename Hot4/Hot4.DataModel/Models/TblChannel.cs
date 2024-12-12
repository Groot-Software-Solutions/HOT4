using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblChannel
{
    [Key]
    public byte ChannelId { get; set; }

    public required string Channel { get; set; }

    public virtual ICollection<Access> TblAccesses { get; set; } = new List<Access>();

    public virtual ICollection<TblTransfer> TblTransfers { get; set; } = new List<TblTransfer>();
}
