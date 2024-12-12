using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblProfileDiscount
{
    [Key]
    public int ProfileDiscountId { get; set; }

    public int ProfileId { get; set; }

    public byte BrandId { get; set; }

    public decimal Discount { get; set; }

    public virtual TblBrand Brand { get; set; }

    public virtual TblProfile Profile { get; set; }
}
