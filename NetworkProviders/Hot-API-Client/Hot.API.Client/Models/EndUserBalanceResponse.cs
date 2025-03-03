using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Models
{
    public class EndUserBalanceResponse : Response
    {
        public decimal MobileBalance { get; set; }
        //public DateTime? WindowPeriod { get; set; }
    }
}
