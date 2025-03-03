using Hot.API.Interface.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Models
{
    public class TransactionsGetResponse : Response
    {
        public List<RechargeTransaction> recharges;

    }
}
