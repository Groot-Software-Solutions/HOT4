using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class TblSms
{
    [Key]
    public long Smsid { get; set; }

    public byte? SmppId { get; set; }

    public byte StateId { get; set; }

    public byte PriorityId { get; set; }

    public bool Direction { get; set; }

    public required string Mobile { get; set; }

    public required string Smstext { get; set; }

    public DateTime Smsdate { get; set; }

    public long? SmsidIn { get; set; }

    public DateTime? InsertDate { get; set; }

    public virtual ICollection<TblSms> InverseSmsidInNavigation { get; set; } = new List<TblSms>();

    public virtual TblPriority Priority { get; set; }

    public virtual TblSmpp? Smpp { get; set; }

    public virtual TblSms? SmsidInNavigation { get; set; }

    public virtual TblState State { get; set; }

    public virtual ICollection<TblRecharge> Recharges { get; set; } = new List<TblRecharge>();
}
