using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblNetwork
{
    [Key]
    public byte NetworkId { get; set; }

    public required string Network { get; set; }

    public required string Prefix { get; set; }

    public virtual ICollection<TblBrand> TblBrands { get; set; } = new List<TblBrand>();
}
