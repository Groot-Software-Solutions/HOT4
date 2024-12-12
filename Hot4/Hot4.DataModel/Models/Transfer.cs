using System.ComponentModel.DataAnnotations;

namespace Hot4.DataModel.Models;

public partial class Transfer
{
    [Key]
    public long TransferId { get; set; }

    public byte ChannelId { get; set; }

    public long PaymentIdFrom { get; set; }

    public long PaymentIdTo { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransferDate { get; set; }

    public long Smsid { get; set; }

    public virtual Channels Channel { get; set; }

    public virtual Payment PaymentIdFromNavigation { get; set; }

    public virtual Payment PaymentIdToNavigation { get; set; }
}
