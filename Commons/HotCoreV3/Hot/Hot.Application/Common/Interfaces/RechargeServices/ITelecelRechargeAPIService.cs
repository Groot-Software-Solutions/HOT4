using Hot.Application.Common.Models.RechargeServiceModels.Telecel;

namespace Hot.Application.Common.Interfaces.RechargeServices;
public interface ITelecelRechargeAPIService
{
    public Task<TelecelRechargeResult> Recharge(string Mobile, decimal Amount, string Reference, Currency currency);


}

