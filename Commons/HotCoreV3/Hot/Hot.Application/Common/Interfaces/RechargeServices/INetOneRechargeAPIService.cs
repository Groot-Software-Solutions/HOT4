using Hot.Application.Common.Models.RechargeServiceModels.NetOne;

namespace Hot.Application.Common.Interfaces.RechargeServices
{
    public interface INetOneRechargeAPIService
    {
        public Task<NetOneRechargeResult> Recharge(string Mobile, decimal Amount, long Reference, Currency currency);
        public Task<NetOneRechargeResult> QueryEndUserBalance(string Mobile);

    }
}
