using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRechargeHelper
    {
        Task<bool> SubmitSelfTopUp(SelfTopUp recharge);
    }
}
