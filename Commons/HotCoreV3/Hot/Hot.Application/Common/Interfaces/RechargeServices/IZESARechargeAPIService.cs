using Hot.Application.Common.Models.RechargeServiceModels.ZESA;

namespace Hot.Application.Common.Interfaces.RechargeServices;

public interface IZESARechargeAPIService
{
    public Task<ZESAAccountQueryResult> QueryZESAAccount(string MeterNumber);
    public Task<ZESAPurchaseTokenResult> PurchaseZesaToken(string MeterNumber, decimal Amount, string Reference, Currency Currency);
}
