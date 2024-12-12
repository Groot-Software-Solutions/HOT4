using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Product
{
    [Key]
    public byte ProductId { get; set; }

    public byte BrandId { get; set; }

    public required string Name { get; set; }

    public int WalletTypeId { get; set; }

    public int ProductStateId { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual ICollection<ProductField> ProductFields { get; set; } = new List<ProductField>();

    public virtual ICollection<ProductMetaData> ProductMetaData { get; set; } = new List<ProductMetaData>();

    public virtual WalletType WalletType { get; set; }
}
