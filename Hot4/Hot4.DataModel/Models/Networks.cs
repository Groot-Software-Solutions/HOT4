using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class Networks
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte NetworkId { get; set; }

    public required string Network { get; set; }

    public required string Prefix { get; set; }

    public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();
}
