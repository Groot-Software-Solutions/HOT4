using Hot.Application.Common.Models.RechargeServiceModels.Econet;

namespace Hot.Application.Common.Interfaces.RechargeServices;

public interface IEconetRechargeAPIService
{
    public Task<EconetRechargeResult> Recharge(string Mobile, decimal Amount, string Reference);
    public Task<EconetBalanceResult> QueryEndUserBalance(string Mobile);
    public Task<EconetRechargeResult> Debit(string Mobile, decimal Amount, string Reference);

}
