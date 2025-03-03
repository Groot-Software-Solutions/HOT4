using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Helpers;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Domain.Entities;
using Hot.Ecocash.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf; 

namespace Hot.Ecocash.Application.Actions;
public record FundWalletWithEcocashDirectCommand
    (string AccessCode, decimal Amount, string TargetMobile, string OnBehalfOf = "Comm Shop",
        EcocashAccounts Account = EcocashAccounts.APIUserAccount) : IRequest<OneOf<Transaction, string, AppException>>;

public class FundWalletWithEcocashDirectCommandHandler : IRequestHandler<FundWalletWithEcocashDirectCommand, OneOf<Transaction, string, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<FundWalletWithEcocashDirectCommandHandler> logger;
    private readonly IEcocashServiceFactory ecocash;

    public FundWalletWithEcocashDirectCommandHandler(IDbContext context, ILogger<FundWalletWithEcocashDirectCommandHandler> logger, IEcocashServiceFactory ecocash)
    {
        this.context = context;
        this.logger = logger;
        this.ecocash = ecocash;
    }

    public async Task<OneOf<Transaction, string, AppException>> Handle(FundWalletWithEcocashDirectCommand request, CancellationToken cancellationToken)
    {
        var accessResponse = await context.Accesss.SelectCodeAsync(request.AccessCode);
        if (accessResponse.IsT1) return CommandHelper.ReturnDbException(accessResponse.AsT1, logger);
        var access = accessResponse.AsT0;

        var CurrentBatch = GetCurrentBankTrxBatchObject(request.Account);
        var bankTrxBatchResponse = await context.BankTrxBatches.GetCurrentBatchAsync(CurrentBatch);
        if (bankTrxBatchResponse.IsT1) return CommandHelper.ReturnDbException(bankTrxBatchResponse.AsT1, logger);
        var bankTrxBatch = bankTrxBatchResponse.AsT0;

        var bankTrx = GetBankTrx(request, access, bankTrxBatch);
        var banktrxResponse = await context.BankTrxs.AddAsync(bankTrx);
        if (banktrxResponse.IsT1) return CommandHelper.ReturnDbException(banktrxResponse.AsT1, logger);
        bankTrx.BankTrxID = banktrxResponse.AsT0;

        var currency = request.Account == EcocashAccounts.FCAAccount ? Currencies.USD_FCA : Currencies.ZiG;
        var ecocashService = ecocash.GetService(request.Account);
        var ecocashResult = await ecocashService.ChargeNumberAsync(request.TargetMobile, bankTrx.BankTrxID.ToString(), request.Amount, "CommShop", "Airtime", currency);

        bankTrx.BankRef = ecocashResult.ValidResponse ? ecocashResult.Item.serverReferenceCode : ecocashResult.ErrorData;
        bankTrx.BankTrxStateID = (byte)(ecocashResult.ValidResponse ? 6 : 5);
        var bankTrxUpdate = await context.BankTrxs.UpdateAsync(bankTrx);

        if (ecocashResult.ValidResponse) return ecocashResult.Item;
        return ecocashResult.ErrorData; ;
    }

    private static BankTrxBatch GetCurrentBankTrxBatchObject(EcocashAccounts account)
    {
        return new BankTrxBatch()
        {
            BankID = 6,
            LastUser = "SMSUser",
            BatchDate = DateTime.Now,
            BatchReference = $"{GetAccountBatchName(account)}Merchant {DateTime.Now:dd mmm yyyy}"
        };
    }

    private static string GetAccountBatchName(EcocashAccounts account)
    {
        return account switch
        {
            EcocashAccounts.MainAccount => "Main",
            EcocashAccounts.ZESAAccount => "Zesa",
            EcocashAccounts.APIUserAccount => "API",
            _ => "SMS"
        };
    }

    private static BankTrx GetBankTrx(FundWalletWithEcocashDirectCommand request, Access access, BankTrxBatch bankTrxBatch)
    {
        var refPrefix = request.Account switch
        {
            EcocashAccounts.ZESAAccount => "Zesa-",
            EcocashAccounts.FCAAccount => "USD-",
            _ => ""
        };

        return new BankTrx()
        {
            BankTrxBatchID = bankTrxBatch.BankTrxBatchID,
            Amount = Math.Round(request.Amount, 2),
            TrxDate = DateTime.Now,
            Identifier = request.TargetMobile,
            RefName = $"{refPrefix}{access.AccessId}",
            Branch = $"API-{access.AccessId}",
            BankRef = "pending",
            Balance = 0,
            BankTrxStateID = 0,
            BankTrxTypeID = 17,
        };
    }
}


