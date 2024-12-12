using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class WalletType
{
    [Key]
    public int WalletTypeId { get; set; }

    public required string WalletName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
