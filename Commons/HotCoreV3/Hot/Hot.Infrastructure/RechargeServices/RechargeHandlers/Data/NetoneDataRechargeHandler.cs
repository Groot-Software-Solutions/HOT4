
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.NetOne;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Data;

public class NetoneUSDDataRechargeHandler : BaseRechargeHandler, IRechargeHandler
{
    public NetoneUSDDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }
    public RechargeType Rechargetype { get; set; } = RechargeType.Data;
    public int BrandId { get; set; } = (int)Brands.NetoneUSDData;
    public Currency Currency { get; set; } = Currency.USD;
    public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        await SetRecharge(recharge, rechargePrepaid);
        await ApplyDiscountRules();
        var service = GetServiceFromProvider<INetoneRechargeDataAPIService>();
        if (service is null) return await UpdateAndReturnServiceNotFound("Netone Service");
        if (rechargePrepaid.Data is null) return await UpdateAndReturnInvalidDataRequest("Netone Service");
        var response = await service.RechargeDataBundle(recharge.Mobile, rechargePrepaid.Data, recharge.RechargeId, recharge.Amount, Currency);
        SetResultFromResponse(response);
        await UpdateRechargeStatusAsync();
        var result = await GetRechargeResultFromResponseAsync(response);
        return result;
    }
    internal void SetResultFromResponse(NetoneDataRechargeResult response)
    {
        Recharge.StateId = (byte)(response.Successful ? 2 : 3);
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"{response.TransactionResult} ;Raw Request: {response.RawResponseData}";
        RechargePrepaid.ReturnCode = response.ReplyCode.ToString();
        RechargePrepaid.InitialWallet = response.InitialWallet;
        RechargePrepaid.FinalWallet = response.FinalWallet; 
        RechargePrepaid.SMS = response.RawResponseData;
    }

    internal async Task<RechargeResult> UpdateAndReturnInvalidDataRequest(string ProductCode)
    {
        Recharge.StateId = 3;
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"Invalid Product code provided - {ProductCode}";
        RechargePrepaid.ReturnCode = "-1";
        await UpdateRechargeStatusAsync();
        return await GetRechargeResultFromResponseAsync(
            new NetoneDataRechargeResult()
            {
                Successful = false, 
                TransactionResult = RechargePrepaid.Narrative,
            });
    }

    internal async Task<RechargeResult> GetRechargeResultFromResponseAsync(NetoneDataRechargeResult response)
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
}

public class NetoneDataRechargeHandler : NetoneUSDDataRechargeHandler, IRechargeHandler
{
    public NetoneDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider) { }
   
    public new int BrandId { get; set; } = (int)Brands.NetoneSMS;
    public new Currency Currency { get; set; } = Currency.ZWG;
    
}

public class NetoneDataSocialRechargeHandler : NetoneUSDDataRechargeHandler, IRechargeHandler
{
    public NetoneDataSocialRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider) { }

    public new int BrandId { get; set; } = (int)Brands.NetoneSocial;
    public new Currency Currency { get; set; } = Currency.ZWG;

}

public class NetoneDataWhatsappRechargeHandler : NetoneUSDDataRechargeHandler, IRechargeHandler
{
    public NetoneDataWhatsappRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider) { }

    public new int BrandId { get; set; } = (int)Brands.NetoneWhatsApp;
    public new Currency Currency { get; set; } = Currency.ZWG;

}

public class NetoneDataOneFiRechargeHandler : NetoneUSDDataRechargeHandler, IRechargeHandler
{
    public NetoneDataOneFiRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider) { }

    public new int BrandId { get; set; } = (int)Brands.OneFi;
    public new Currency Currency { get; set; } = Currency.ZWG;

}

public class NetoneDataOneFusionRechargeHandler : NetoneUSDDataRechargeHandler, IRechargeHandler
{
    public NetoneDataOneFusionRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider) { }

    public new int BrandId { get; set; } = (int)Brands.OneFusion;
    public new Currency Currency { get; set; } = Currency.ZWG;

}
 
