using Hot.Application.Common.Models.RechargeServiceModels.NetOne;

namespace Hot.Application.Common.Interfaces.RechargeServices;

public interface INetoneRechargeDataAPIService
{
    public Task<NetoneDataRechargeResult> RechargeDataBundle(string Mobile, string ProductCode, long Reference, decimal Amount,Currency currency);
    public Task<NetoneBundleQueryResult> QueryBundles();

}