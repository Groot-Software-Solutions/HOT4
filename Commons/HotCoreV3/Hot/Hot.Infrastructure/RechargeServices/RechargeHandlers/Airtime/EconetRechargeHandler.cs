using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using OneOf;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime;
public class EconetRechargeHandler : BaseRechargeHandler, IRechargeHandler
{
    public EconetRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public int BrandId { get; set; } = (int)Brands.EconetPlatform;
    public RechargeType Rechargetype { get; set; } = RechargeType.Airtime;

    public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        await SetRecharge(recharge, rechargePrepaid);
        await ApplyDiscountRules();
        var service = GetServiceFromProvider<IEconetRechargeAPIService>();
        if (service is null) return await UpdateAndReturnServiceNotFound("Econet Service");
        var response = await service.Recharge(recharge.Mobile, recharge.Amount, GetReference());
        SetResultFromResponse(response);
        await UpdateRechargeStatusAsync();
        var result = await GetRechargeResultFromResponseAsync(response);
        return result;
    }
    internal override string GetReference()
    {
        return Recharge.RechargeId.ToString();
    }

    internal async Task<RechargeResult> GetRechargeResultFromResponseAsync(EconetRechargeResult response)
    {
        return new RechargeResult()
        {
            Recharge = Recharge,
            RechargePrepaid = RechargePrepaid,
            ErrorData = response.Successful ? null : response.RawResponseData,
            Message = response.TransactionResult ?? "",
            Successful = response.Successful,
            WalletBalance = await GetWalletBalanceAsync(),
        };
    }

    internal void SetResultFromResponse(EconetRechargeResult response)
    {
        Recharge.StateId = (byte)(response.Successful ? 2 : 3);
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"{response.TransactionResult} - {response.RawResponseData}";
        RechargePrepaid.ReturnCode = response.Successful ? "1" : "0";
        RechargePrepaid.InitialBalance = response.InitialBalance;
        RechargePrepaid.FinalBalance = response.FinalBalance;
        RechargePrepaid.Data = response.RawResponseData;
        RechargePrepaid.InitialWallet = response.InitialWallet; 
        RechargePrepaid.FinalWallet = response.FinalWallet;

    }

}


public class Econet078RechargeHandler : EconetRechargeHandler, IRechargeHandler
{
    public Econet078RechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }
    public new int BrandId { get; set; } = (int)Brands.Econet078;
}


