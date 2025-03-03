using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Telone;
using Hot.Domain.Entities;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Data;
public class TeloneRechargeHandler : BaseRechargeHandler, IRechargeHandler
{
    public TeloneRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }
    public RechargeType Rechargetype { get; set; } = RechargeType.Data;
    public int BrandId { get; set; } = (int)Brands.TeloneBroadband;
    public Currency Currency { get; set; } = Currency.ZWG;
    public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        await SetRecharge(recharge, rechargePrepaid);
        await ApplyDiscountRules();
        var service = GetServiceFromProvider<ITeloneDataAPIService>();
        if (service is null) return await UpdateAndReturnServiceNotFound("Telone Service");
        var isValidBundle = (rechargePrepaid.Data ?? "").TryParse(out int ProductId);
        if (!isValidBundle) return await UpdateAndReturnInvalidDataRequest(rechargePrepaid.Data ?? "");

        var response = await service.RechargeDataBundle(recharge.Mobile, ProductId, (int)recharge.RechargeId, Currency);
        SetResultFromResponse(response);
        await UpdateRechargeStatusAsync();
        var result = await GetRechargeResultFromResponseAsync(response);
        return result;
    }

    private async Task<RechargeResult> GetRechargeResultFromResponseAsync(TeloneRechargeResult response)
    {
        return new RechargeResult()
        {
            Recharge = Recharge,
            RechargePrepaid = RechargePrepaid,
            ErrorData = response.Successful ? null : response.RawResponseData,
            Message = response.TransactionResult ?? "",
            Successful = response.Successful,
            WalletBalance = await GetWalletBalanceAsync(),
            Data = response.Vouchers,
        };
    }

    private void SetResultFromResponse(TeloneRechargeResult response)
    {
        Recharge.StateId = (byte)(response.Successful ? 2 : 3);
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"{response.TransactionResult} | Reference: {response.Reference}| RawResponse: {response.RawResponseData}";
        RechargePrepaid.ReturnCode = response.ResponseCode;
        RechargePrepaid.InitialWallet = response.InitialWallet;
        RechargePrepaid.FinalWallet = response.FinalWallet;
        RechargePrepaid.SMS = response.RawResponseData;
    }

    private async Task<RechargeResult> UpdateAndReturnInvalidDataRequest(string ProductCode)
    {
        Recharge.StateId = 3;
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"Invalid Product code provided - {ProductCode}";
        RechargePrepaid.ReturnCode = "-1";
        await UpdateRechargeStatusAsync();
        return await GetRechargeResultFromResponseAsync(
            new TeloneRechargeResult()
            {
                Successful = false,
                TransactionResult = RechargePrepaid.Narrative,
            });
    }
}

public class TeloneUSDRechargeHandler : TeloneRechargeHandler, IRechargeHandler
{
    public TeloneUSDRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
        Currency = Currency.USD;
    }

    public new int BrandId { get; set; } = (int)Brands.TeloneUSD;

}

public class TeloneBillRechargeHandler : TeloneRechargeHandler, IRechargeHandler
{
    public TeloneBillRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.TeloneBillPayment;

}

public class TeloneLTERechargeHandler : TeloneRechargeHandler, IRechargeHandler
{
    public TeloneLTERechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.TeloneLTE;

}

public class TeloneVoiceRechargeHandler : TeloneRechargeHandler, IRechargeHandler
{
    public TeloneVoiceRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.TeloneVoice;

}

public class TeloneVoipRechargeHandler : TeloneRechargeHandler, IRechargeHandler
{
    public TeloneVoipRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }

    public new int BrandId { get; set; } = (int)Brands.TeloneVoip;

}

