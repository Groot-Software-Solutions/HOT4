using Hot.Application.Common.Models.RechargeServiceModels.Econet;

namespace Hot.Application.Common.Interfaces.RechargeServices;

public interface IEconetRechargePrepaidAPIService
{
    public Task<EconetRechargeResult> RechargeAirtime(string Mobile, decimal Amount, long Reference, Currency currency); 

}
