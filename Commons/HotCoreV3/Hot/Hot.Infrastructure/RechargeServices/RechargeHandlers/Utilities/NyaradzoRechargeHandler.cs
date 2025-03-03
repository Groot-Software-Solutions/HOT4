using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Utilities;

public class NyaradzoRechargeHandler : BaseRechargeHandler, IRechargeHandler
{
    public NyaradzoRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider)
        : base(dbcontext, serviceProvider)
    {
    }

    public int BrandId { get; set; } = (int)Brands.Nyaradzo;
    public RechargeType Rechargetype { get; set; } = RechargeType.Utility;
    public Currency Currency { get; set; } = Currency.ZWG;
    public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync
        (Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        await SetRecharge(recharge, rechargePrepaid);
        await ApplyDiscountRules();
        var request = GetRequestForRecharge();
        var service = GetServiceFromProvider<INyaradzoRechargeAPIService>();
        if (service is null) return await UpdateAndReturnServiceNotFound("Nyaradzo Service");
        var response = await service.ProcessPayment(request,Currency);
        SetResultFromResponse(response);
        await UpdateRechargeStatusAsync();
        var result = await GetRechargeResultFromResponseAsync(response);
        return result;
    }

    private NyaradzoPaymentRequest GetRequestForRecharge()
    {
        return new NyaradzoPaymentRequest(
                        Recharge.Mobile,
                        GetReference(),
                        Recharge.Amount,
                        RechargePrepaid.InitialBalance,
                        1,
                        DateTime.Now
                        );
    }

    internal override string GetReference()
    {
        return $"Hot-{Recharge.RechargeId}";
    }

    private async Task<RechargeResult> GetRechargeResultFromResponseAsync(NyaradzoResult response)
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

    private void SetResultFromResponse(NyaradzoResult response)
    {
        Recharge.StateId = (byte)(response.Successful ? 2 : 3);
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.InitialBalance = (response.Account ?? new()).Balance.ToDecimal() - Recharge.Amount;
        RechargePrepaid.FinalBalance = (response.Account ?? new()).Balance.ToDecimal();
        RechargePrepaid.Narrative = response.RawResponseData??"";
        RechargePrepaid.ReturnCode = response.Successful ? "1" : "0";
    }
}


public class NyaradzoRechargeUSDHandler : NyaradzoRechargeHandler, IRechargeHandler
{
    public NyaradzoRechargeUSDHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
        Currency= Currency.USD; 
    }

    public new int BrandId { get; set; } = (int)Brands.NyaradzoUSD; 
    
}