using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Models
{
    public class EndUserBalanceResponse : Response
    {
        public string MobileBalance;
        public DateTime? WindowPeriod;
    }
}
