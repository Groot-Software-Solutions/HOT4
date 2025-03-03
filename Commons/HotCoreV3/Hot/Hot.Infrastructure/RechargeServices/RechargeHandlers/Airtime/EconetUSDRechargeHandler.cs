using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using OneOf;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime;

public class EconetUSDRechargeHandler : EconetRechargeHandler, IRechargeHandler
{
    public EconetUSDRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.EconetUSD; 
    public new async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        await SetRecharge(recharge, rechargePrepaid);
        await ApplyDiscountRules();
        var airtimeService = GetServiceFromProvider<IEconetRechargePrepaidAPIService>();
        var dataService = GetServiceFromProvider<IEconetRechargeDataAPIService>();
        if (airtimeService is null) return await UpdateAndReturnServiceNotFound("Econet Service");
        if (dataService is null) return await UpdateAndReturnServiceNotFound("Econet Data Service");
        var response = !string.IsNullOrEmpty(rechargePrepaid.Data)
            ? MapToRechargeResponse(await dataService.RechargeDataBundle(recharge.Mobile, rechargePrepaid.Data, recharge.RechargeId, Currency.USD))
            : await airtimeService.RechargeAirtime(recharge.Mobile, recharge.Amount, recharge.RechargeId, Currency.USD);
        SetResultFromResponse(response);
        await UpdateRechargeStatusAsync();
        var result = await GetRechargeResultFromResponseAsync(response);
        return result;
    }

    private static EconetRechargeResult MapToRechargeResponse(EconetDataRechargeResult result)
    {
        return new()
        {
            RawResponseData = result.RawResponseData,
            FinalWallet = result.FinalWallet,
            InitialWallet = result.InitialWallet,
            ResponseCode = result.StatusCode,
            Successful = result.Successful,
            TransactionResult = result.TransactionResult,
            FinalBalance = 0,
            InitialBalance = 0,

        };
    }

    internal new void SetResultFromResponse(EconetRechargeResult response)
    {
        Recharge.StateId = (byte)(response.Successful ? 2 : 3);
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"{response.TransactionResult} - {response.RawResponseData}";
        RechargePrepaid.ReturnCode = response.ResponseCode;
        RechargePrepaid.InitialWallet = response.InitialWallet;
        RechargePrepaid.FinalWallet = response.FinalWallet;
        RechargePrepaid.SMS = response.RawResponseData;

    }
}
