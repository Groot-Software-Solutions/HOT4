using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblRecharge
{
    [Key]
    public long RechargeId { get; set; }

    public byte StateId { get; set; }

    public long AccessId { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public required string Mobile { get; set; }

    public byte BrandId { get; set; }

    public DateTime RechargeDate { get; set; }

    public DateTime? InsertDate { get; set; }

    public virtual Access Access { get; set; }

    public virtual TblBrand Brand { get; set; }

    public virtual TblState State { get; set; }

    public virtual TblRechargePrepaid? TblRechargePrepaid { get; set; }

    public virtual ICollection<TblReservation> TblReservations { get; set; } = new List<TblReservation>();

    public virtual ICollection<TblSelfTopUp> TblSelfTopUps { get; set; } = new List<TblSelfTopUp>();

    //  public virtual ICollection<TblPin> Pins { get; set; } = new List<TblPin>();

    //  public virtual ICollection<TblSm> Sms { get; set; } = new List<TblSm>();
}
