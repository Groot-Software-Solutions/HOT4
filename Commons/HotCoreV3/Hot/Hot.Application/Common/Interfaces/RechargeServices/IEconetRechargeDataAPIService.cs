using Hot.Application.Common.Models.RechargeServiceModels.Econet;

namespace Hot.Application.Common.Interfaces.RechargeServices;

public interface IEconetRechargeDataAPIService
{
    public Task<EconetDataRechargeResult> RechargeDataBundle(string Mobile, string ProductCode, long Reference, Currency currency); 
    public Task<EconetBundleQueryResult> QueryBundles(); 

}
