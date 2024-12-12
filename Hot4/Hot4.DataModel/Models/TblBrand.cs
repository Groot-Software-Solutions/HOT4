using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblBrand
{
    [Key]
    public byte BrandId { get; set; }

    public byte NetworkId { get; set; }

    public required string BrandName { get; set; }

    public required string BrandSuffix { get; set; }

    public int? WalletTypeId { get; set; }

    public virtual TblNetwork Network { get; set; }

    public virtual ICollection<TblPin> TblPins { get; set; } = new List<TblPin>();

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();

    public virtual ICollection<TblProfileDiscount> TblProfileDiscounts { get; set; } = new List<TblProfileDiscount>();

    public virtual ICollection<TblRecharge> TblRecharges { get; set; } = new List<TblRecharge>();

    public virtual ICollection<TblReservation> TblReservations { get; set; } = new List<TblReservation>();

    public virtual ICollection<TblSelfTopUp> TblSelfTopUps { get; set; } = new List<TblSelfTopUp>();

    public virtual ICollection<TblSubscriber> TblSubscribers { get; set; } = new List<TblSubscriber>();
}
