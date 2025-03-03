using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Helpers;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Ecocash.Domain.Entities;
using Hot.Ecocash.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OneOf;

namespace Hot.External.Ecocash.Application.Actions;

public record FundWalletWithEcocashWithServiceCommand
    (string AccessCode, decimal Amount, string TargetMobile, string OnBehalfOf = "Comm Shop",
        EcocashAccounts Account = EcocashAccounts.APIUserAccount) : IRequest<OneOf<Transaction, string, AppException>>;

public class FundWalletWithEcocashWithServiceCommandHandler : IRequestHandler<FundWalletWithEcocashWithServiceCommand, OneOf<Transaction, string, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<FundWalletWithEcocashWithServiceCommandHandler> logger;

    public FundWalletWithEcocashWithServiceCommandHandler(IDbContext context, ILogger<FundWalletWithEcocashWithServiceCommandHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<OneOf<Transaction, string, AppException>> Handle(FundWalletWithEcocashWithServiceCommand request, CancellationToken cancellationToken)
    {
        var accessResponse = await context.Accesss.SelectCodeAsync(request.AccessCode);
        if (accessResponse.IsT1) return accessResponse.AsT1.ReturnDbException(logger);
        var access = accessResponse.AsT0;

        var CurrentBatch = GetCurrentBankTrxBatchObject(request.Account);
        var bankTrxBatchResponse = await context.BankTrxBatches.GetCurrentBatchAsync(CurrentBatch);
        if (bankTrxBatchResponse.IsT1) return bankTrxBatchResponse.AsT1.ReturnDbException(logger);
        var bankTrxBatch = bankTrxBatchResponse.AsT0;

        var bankTrx = GetBankTrx(request, access, bankTrxBatch);
        var banktrxResponse = await context.BankTrxs.AddAsync(bankTrx);
        if (banktrxResponse.IsT1) return banktrxResponse.AsT1.ReturnDbException(logger);
        bankTrx.BankTrxID = banktrxResponse.AsT0;

        return new Transaction()
        {
            id = (int)bankTrx.BankTrxID,
            clientCorrelator = bankTrx.BankTrxID.ToString(),
            ecocashReference = "Pending",
            merchantCode = "",
            merchantNumber = "",
            merchantPin = "",
            endTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
            notifyUrl = "",
            paymentAmount = new(),
            originalServerReferenceCode = bankTrx.BankTrxID.ToString(),
            transactionOperationStatus = "INITIATED",
            referenceCode = "PENDING",
        };
    }

    private static BankTrxBatch GetCurrentBankTrxBatchObject(EcocashAccounts account)
    {
        return new BankTrxBatch()
        {
            BankID = 6,
            LastUser = "SMSUser",
            BatchDate = DateTime.Now,
            BatchReference = $"{GetAccountBatchName(account)}Merchant {DateTime.Now:dd MMM yyyy}"
        };
    }

    private static string GetAccountBatchName(EcocashAccounts account)
    {
        return account switch
        {
            EcocashAccounts.MainAccount => "Main",
            EcocashAccounts.ZESAAccount => "Zesa",
            EcocashAccounts.USDUtilityAccount => "UUSD",
            EcocashAccounts.APIUserAccount => "API",
            _ => "SMS"
        };
    }

    private static BankTrx GetBankTrx(FundWalletWithEcocashWithServiceCommand request, Access access, BankTrxBatch bankTrxBatch)
    {
        var refPrefix = request.Account switch
        {
            EcocashAccounts.ZESAAccount => "Zesa-",
            EcocashAccounts.FCAAccount => "USD-",
            EcocashAccounts.USDUtilityAccount => "UUSD-",
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

