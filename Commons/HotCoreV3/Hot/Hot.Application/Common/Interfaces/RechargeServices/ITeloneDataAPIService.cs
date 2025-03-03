
using Hot.Application.Common.Models.RechargeServiceModels.Telone;

namespace Hot.Application.Common.Interfaces.RechargeServices;

public interface ITeloneDataAPIService
{
    public Task<TeloneRechargeResult> RechargeDataBundle(string Mobile, int ProductId, int Reference, Currency currency);
    public Task<TeloneBundleQueryResult> QueryBundles(Currency currency);
    public Task<TeloneCustomerResult> QueryAccount(string AccountNumber);
}


