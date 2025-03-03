using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.ZESA;
using Hot.Domain.Entities;
using Hot.Domain.Enums; 

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Utilities;
public class ZesaRechargeHandler : BaseRechargeHandler, IRechargeHandler
{
    public ZesaRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider) { }
    public RechargeType Rechargetype { get; set; } = RechargeType.Utility;
    public int BrandId { get; set; } = (int)Brands.ZETDC;

    public Currency Currency { get; set; } = Currency.ZWG;


    public async Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid)
    {
        await SetRecharge(recharge, rechargePrepaid);
        await ApplyDiscountRules();
        var service = GetServiceFromProvider<IZESARechargeAPIService>();
        if (service is null) return await UpdateAndReturnServiceNotFound("ZESA Service");
        var response = await service.PurchaseZesaToken(recharge.Mobile, recharge.Amount, rechargePrepaid.Reference, Currency);
        SetResultFromResponse(response);
        await UpdateRechargeStatusAsync();
        if (response.Successful)
        {
            await SendTokensToClientsAsync(response);
            await SaveTokensToDatabase(response);
        }
        var result = await GetRechargeResultFromResponseAsync(response);
        return result;
    }

    internal override string GetReference()
    {
        return Recharge.RechargeId.ToString();
    }
    internal async Task<RechargeResult> GetRechargeResultFromResponseAsync(ZESAPurchaseTokenResult response)
    {
        return new RechargeResult()
        {
            Recharge = Recharge,
            RechargePrepaid = RechargePrepaid,
            ErrorData = response.RawResponseData,
            Message = response.TransactionResult ?? "",
            Successful = response.Successful,
            WalletBalance = await GetWalletBalanceAsync(),
            Data = response,
        };
    }

    internal void SetResultFromResponse(ZESAPurchaseTokenResult response)
    {
        Recharge.StateId = (byte)(response.Successful ? 2 : response.ReturnCode == 312 ? 4 : 3);
        Recharge.RechargeDate = DateTime.Now;
        RechargePrepaid.Narrative = $"{response.TransactionResult} - {response.RawResponseData}";
        RechargePrepaid.ReturnCode = response.ReturnCode.ToString();
        RechargePrepaid.InitialWallet = response.InitialWallet;
        RechargePrepaid.FinalWallet = response.FinalWallet;
        RechargePrepaid.Data = response.RawResponseData; 

    }

    private async Task SendTokensToClientsAsync(ZESAPurchaseTokenResult response)
    {
        try
        {
            if (RechargePrepaid.SMS is null) return;
            var tokens = response.PurchaseToken.Tokens;
            if (!tokens.Any()) return;
            var dbContext = GetServiceFromProvider<IDbContext>();
            if (dbContext is null) return;
            var templateResult = await dbContext.Templates.GetAsync((int)Templates.SuccessfulZESAStandardTemplate);
            if (templateResult.IsT1) return;
            var template = templateResult.AsT0;

            Template smsText = template;
            var tokenData = string.Join("\n", tokens.Select(t => $"Token: {t.Token}").ToList());
            var t = response.PurchaseToken.Tokens.First();
            smsText
                .SetToken(tokenData)
                .SetMeter(response.PurchaseToken.MeterNumber)
                .SetTotalAmount((decimal)(t.NetAmount + t.Arrears + t.Levy + t.TaxAmount))
                .SetAmount((decimal)response.PurchaseToken.Amount)
                .SetTotalPaid((decimal)response.PurchaseToken.Amount)
                .SetUnits((decimal)t.Units)
                .SetNetAmount((decimal)t.NetAmount).SetDebt((decimal)t.Arrears)
                .SetLevy((decimal)t.Levy).SetTax((decimal)t.TaxAmount)
                .SetDate(DateTime.Now)
                .SetCurrency(response.CustomerInfo.Currency)
                .SetCurrencyPaid(Currency);

            var sms = new SMS()
            {
                Direction = false,
                Mobile = RechargePrepaid.SMS,
                Priority = new Priority() { PriorityId = (byte)Priorities.Normal },
                State = new State() { StateID = (byte)States.Pending },
                SMSID_In = null,
                SMSText = smsText.TemplateText,
            };

            _ = await dbContext.SMSs.AddAsync(sms);
        }
        catch (Exception)
        {
            return;
        }

    }

    private async Task SaveTokensToDatabase(ZESAPurchaseTokenResult response)
    {
        var dbContext = GetServiceFromProvider<IDbContext>();
        if (dbContext is null) return;
        var transaction = await dbContext.BeginTransactionAsync();
        if (transaction is null) return;
        try
        {
            var pinBatch = new PinBatch()
            {
                BatchDate = DateTime.Now,
                Name = $"ZESA-PinBatch-{DateTime.Now:MMDDYYYY}",
                PinBatchTypeId = 8, // ZESA 
            };
            var pinBatchResponse = dbContext.PinBatches.Add(pinBatch, transaction.Item1, transaction.Item2);
            if (pinBatchResponse.IsT0) throw pinBatchResponse.AsT1;
            pinBatch.PinBatchId = pinBatchResponse.AsT0;

            var pins = response.PurchaseToken.Tokens.Select(t => new Pin()
            {
                BrandId = (byte)BrandId,
                PinBatchId = pinBatch.PinBatchId,
                PinNumber = t.Token,
                PinValue = ((decimal)(t.NetAmount + t.TaxAmount + t.Levy)),
                PinExpiry = DateTime.Now.AddMonths(3),
                PinRef = t.ZesaReference,
                PinStateId = (byte)PinStates.SoldHotRecharge,
            });

            pins.ToList().ForEach(pin =>
            {
                var pinResponse = dbContext.Pins.Add(pin, transaction.Item1, transaction.Item2);
                if (pinResponse.IsT1) throw pinResponse.AsT1;
            });

            dbContext.CompleteTransaction(transaction.Item2);
        }
        catch (Exception)
        {
            dbContext.RollbackTransaction(transaction.Item2);
            return;
        }


    }

}

public class ZesaUSDRechargeHandler : ZesaRechargeHandler, IRechargeHandler
{
    public ZesaUSDRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
        Currency = Currency.USD;
    }

    public new int BrandId { get; set; } = (int)Brands.ZETDCUSD;

}
