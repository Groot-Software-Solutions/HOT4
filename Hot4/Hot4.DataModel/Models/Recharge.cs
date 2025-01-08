using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Recharge
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

    public virtual Brand Brand { get; set; }

    public virtual States State { get; set; }

    public virtual RechargePrepaid? RechargePrepaid { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<SelfTopUp> SelfTopUps { get; set; } = new List<SelfTopUp>();

    // public virtual RechargePin RechargePin { get; set; }
    //  public virtual SmsRecharge SmsRecharge { get; set; }
}
