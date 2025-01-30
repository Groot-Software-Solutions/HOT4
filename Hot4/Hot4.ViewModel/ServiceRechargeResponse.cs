using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.ViewModel
{
    public class ServiceRechargeResponse
    {
        public ServiceRechargeResponse(RechargePrepaidModel iRechargePrepaid)
        {
            RechargePrepaid = iRechargePrepaid;
        }

        public RechargePrepaidModel RechargePrepaid { get; set; }

        public string CustomCustomerCreditSuccessSMS { get; set; }
    }
}
