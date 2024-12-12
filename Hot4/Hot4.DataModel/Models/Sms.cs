using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Sms
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

    public virtual ICollection<Sms> InverseSmsidInNavigation { get; set; } = new List<Sms>();

    public virtual Priorities Priority { get; set; }

    public virtual Smpp? Smpp { get; set; }

    public virtual Sms? SmsidInNavigation { get; set; }

    public virtual States State { get; set; }

    public virtual ICollection<Recharge> Recharges { get; set; } = new List<Recharge>();
}
