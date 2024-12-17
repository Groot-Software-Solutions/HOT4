using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class Channels
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte ChannelId { get; set; }

    public required string Channel { get; set; }

    public virtual ICollection<Access> Accesses { get; set; } = new List<Access>();

    public virtual ICollection<Transfer> Transfers { get; set; } = new List<Transfer>();
}
