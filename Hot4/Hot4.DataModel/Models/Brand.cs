using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models;

public partial class Brand
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public byte BrandId { get; set; }

    public byte NetworkId { get; set; }

    public required string BrandName { get; set; }

    public required string BrandSuffix { get; set; }

    public int? WalletTypeId { get; set; }

    public virtual Networks Network { get; set; }

    public virtual ICollection<Pins> Pins { get; set; } = new List<Pins>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<ProfileDiscount> ProfileDiscounts { get; set; } = new List<ProfileDiscount>();

    public virtual ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<SelfTopUp> SelfTopUps { get; set; } = new List<SelfTopUp>();

    public virtual ICollection<Subscriber> Subscribers { get; set; } = new List<Subscriber>();
    public virtual ICollection<Bundle> Bundle { get; set; } = new List<Bundle>();
}
