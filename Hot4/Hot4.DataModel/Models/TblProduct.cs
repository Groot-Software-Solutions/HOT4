using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblProduct
{
    [Key]
    public byte ProductId { get; set; }

    public byte BrandId { get; set; }

    public required string Name { get; set; }

    public int WalletTypeId { get; set; }

    public int ProductStateId { get; set; }

    public virtual TblBrand Brand { get; set; }

    public virtual ICollection<TblProductField> TblProductFields { get; set; } = new List<TblProductField>();

    public virtual ICollection<TblProductMetaData> TblProductMetaData { get; set; } = new List<TblProductMetaData>();

    public virtual TblWalletType WalletType { get; set; }
}
