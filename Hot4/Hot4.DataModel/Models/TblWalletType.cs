using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblWalletType
{
    [Key]
    public int WalletTypeId { get; set; }

    public required string WalletName { get; set; }

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();
}
