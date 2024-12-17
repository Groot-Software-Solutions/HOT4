using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class WalletType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int WalletTypeId { get; set; }

    public required string WalletName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
