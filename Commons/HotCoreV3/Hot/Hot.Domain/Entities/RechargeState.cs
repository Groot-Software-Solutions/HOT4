using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Domain.Entities
{
    public class RechargeState
    {
       

        public byte Id { get; set; }
        public string RechargeStateText { get; set; } = string.Empty;
    }
}
