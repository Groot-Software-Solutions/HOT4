using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZesaAPI;

namespace Hot4.Service.Abstract
{
    public interface IZesaService
    {
        Task<ServiceRechargeResponse> CompleteZesaAsync(RechargeModel recharge, PurchaseToken tokenResponse , long smsId);
    }
}
