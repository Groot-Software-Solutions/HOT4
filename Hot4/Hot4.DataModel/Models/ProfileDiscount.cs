using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class ProfileDiscount
{
    [Key]
    public int ProfileDiscountId { get; set; }

    public int ProfileId { get; set; }

    public byte BrandId { get; set; }

    public decimal Discount { get; set; }

    public virtual Brand Brand { get; set; }

    public virtual Profile Profile { get; set; }
}
