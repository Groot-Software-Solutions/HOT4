using Horizon.XmlRpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Econet.Prepaid.Interfaces.APIEndpoints;
public interface IAccountBalanceEnquiry
{
    [XmlRpcMethod("account_balance_enquiry")]
    object account_balance_enquiry(string username, string password);
}
