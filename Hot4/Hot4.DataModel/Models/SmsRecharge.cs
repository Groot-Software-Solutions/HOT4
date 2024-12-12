using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models
{
    [PrimaryKey(nameof(RechargeId), nameof(SmsId))]
    public partial class SmsRecharge
    {
        [Column(Order = 0)]
        public long RechargeId { get; set; }
        public Recharge Recharge { get; set; }
        [Column(Order = 1)]
        public long SmsId { get; set; }
        public Sms Sms { get; set; }
    }
}
