using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;

namespace Hot.Application.Common.Interfaces.RechargeServices;
public interface INyaradzoRechargeAPIService
{
    public Task<NyaradzoResult> QueryAccount(string PolicyNumber);
    public Task<NyaradzoResult> ProcessPayment(NyaradzoPaymentRequest Payment, Currency currency);
    public Task<NyaradzoResult> Reversal(string Reference);

}
