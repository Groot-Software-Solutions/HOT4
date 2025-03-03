
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Data;

public class EconetDataRechargeHandler : BaseRechargeHandler, IRechargeHandler
{
    public EconetDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }
    public RechargeType Rechargetype { get; set; } = RechargeType.Data;
    public int BrandId { get; set; } = (int)Brands.EconetData;
    public Currency Currency { get; set; } = Currency.ZWG;
    public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        await SetRecharge(recharge, rechargePrepaid);
        await ApplyDiscountRules();
        var service = GetServiceFromProvider<IEconetRechargeDataAPIService>();
        if (service is null) return await UpdateAndReturnServiceNotFound("Econet Service");
        if (rechargePrepaid.Data is null) return await UpdateAndReturnInvalidDataRequest("Econet Service");
        var response = await service.RechargeDataBundle(recharge.Mobile, rechargePrepaid.Data, recharge.RechargeId, Currency);
        SetResultFromResponse(response);
        await UpdateRechargeStatusAsync();
        var result = await GetRechargeResultFromResponseAsync(response);
        return result;
    }
    internal void SetResultFromResponse(EconetDataRechargeResult response)
    {
        Recharge.StateId = (byte)(response.Successful ? 2 : 3);
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"{response.TransactionResult} ;Serial:{response.Reference};ProductCode:{response.ProductCode};Raw Request: {response.RawResponseData}";
        RechargePrepaid.ReturnCode = response.StatusCode;
        RechargePrepaid.InitialWallet = response.InitialWallet;
        RechargePrepaid.FinalWallet = response.FinalWallet;
        RechargePrepaid.Data = response.ProductCode;
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
            new EconetDataRechargeResult()
            {
                Successful = false,
                StatusCode = "998",
                TransactionResult = RechargePrepaid.Narrative,
            });
    }

    internal async Task<RechargeResult> GetRechargeResultFromResponseAsync(EconetDataRechargeResult response)
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

public class EconetInstagramDataRechargeHandler : EconetDataRechargeHandler, IRechargeHandler
{
    public EconetInstagramDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.EconetInstagram;

}

public class EconetBBDataRechargeHandler : EconetDataRechargeHandler, IRechargeHandler
{
    public EconetBBDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.EconetBB;

}

public class EconetTXTDataRechargeHandler : EconetDataRechargeHandler, IRechargeHandler
{
    public EconetTXTDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.EconetTXT;

}

public class EconetTextDataRechargeHandler : EconetDataRechargeHandler, IRechargeHandler
{
    public EconetTextDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.Text;

}

public class EconetTwitterDataRechargeHandler : EconetDataRechargeHandler, IRechargeHandler
{
    public EconetTwitterDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.EconetTwitter;

}

public class EconetWhatsAppDataRechargeHandler : EconetDataRechargeHandler, IRechargeHandler
{
    public EconetWhatsAppDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.EconetWhatsApp;

}

public class EconetFacebookDataRechargeHandler : EconetDataRechargeHandler, IRechargeHandler
{
    public EconetFacebookDataRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.EconetFacebook;

}
