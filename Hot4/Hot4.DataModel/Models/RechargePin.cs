using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.DataModel.Models
{
    [PrimaryKey(nameof(RechargeId), nameof(PinId))]
    public partial class RechargePin
    {
        [Column(Order = 0)]
        public long RechargeId { get; set; }
        public Recharge Recharge { get; set; }
        [Column(Order = 1)]
        public long PinId { get; set; }
        public Pins Pin { get; set; }
    }
}
