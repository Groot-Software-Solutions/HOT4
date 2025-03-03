using Hot.Application.Actions.SelfTopUpActions;
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Extensions;
using Hot.Application.Common.Helpers;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using Hot.Domain.Enums;
using Hot.Ecocash.Application.Common.Extensions;
using Hot.Ecocash.Application.Common.Interfaces;
using Hot.Ecocash.Domain.Entities;
using Hot.Ecocash.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf; 

namespace Hot.Ecocash.Application.Actions;

public record CompleteEcocashCommand(Transaction Item, string LastUser = "System") : IRequest<OneOf<CompleteBankTrxResult, NotFoundException, AppException>>;
public class CompleteEcocashCommandHandler :
    IRequestHandler<CompleteEcocashCommand, OneOf<CompleteBankTrxResult, NotFoundException, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<CompleteEcocashCommandHandler> logger;
    private readonly IMediator mediator;
    private readonly IEcocashServiceFactory ecocashFactory;

    public CompleteEcocashCommandHandler(IDbContext context, ILogger<CompleteEcocashCommandHandler> logger, IMediator mediator, IEcocashServiceFactory ecocashFactory)
    {
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
        this.ecocashFactory = ecocashFactory;
    }

    public async Task<OneOf<CompleteBankTrxResult, NotFoundException, AppException>> Handle(CompleteEcocashCommand request, CancellationToken cancellationToken)
    {
        try
        {

            BankTrx banktrx = await GetBankTrx(request);
            if (banktrx.HasTransactionBeenProcessed())
                return new CompleteBankTrxResult(false, "Transaction has been processed");

            string transactionStatus = request.Item.transactionOperationStatus;
            if (HasExpired(transactionStatus, banktrx)) return await UpdateTransactiontoFailed(banktrx, request.Item);
            if (transactionStatus == "TRANSACTION REVERSED") return await UpdateTransactiontoFailed(banktrx, request.Item);
            if (transactionStatus != "COMPLETED") return new CompleteBankTrxResult(false, $"Waiting to for timeout, current state is {transactionStatus}");

            await ChangeBankTrxStateToProcessing(banktrx);

            Access access = await GetAccess(banktrx);
            Account account = await GetAccount(access);
            Payment payment = request.Item.CreatePayment(banktrx, access, request.LastUser);

            EcocashAccounts ecocashAccount = ecocashFactory.GetEcocashAccountByMerchant(request.Item.merchantCode);
            if (CheckCurrencyMatch(payment, ecocashAccount) == false)
                return new MobileWalletPaymentException("Currency Mismatch", $"{ecocashAccount.Name()}").LogAndReturnError(logger);

            var paymentResponse = await context.Payments.AddAsync(payment);
            if (paymentResponse.IsT1) return paymentResponse.AsT1.LogAndReturnError(logger);

            await UpdateBankTrxWithDetails(banktrx, payment, paymentResponse,request.Item);

            account = (await context.Accounts.GetAsync((int)account.AccountID)).AsT0;

            await SendNotificationSMS(request, banktrx, access, account, payment);

            if (banktrx.IsTransactionSentByAPI()) _ = await mediator.Send(new SendEcocashConfirmationToAPIClient(request.Item, account), cancellationToken);
            if (banktrx.IsSelfTopUpTransaction(context, out SelfTopUp? selftopup))
            {
                var selftopUpResult = await mediator.Send(new UpdateSelfTopUpStateCommand(selftopup ?? new(), banktrx), cancellationToken);
            }
            return new CompleteBankTrxResult(true, banktrx, payment);

        }
        catch (AppException ex)
        {
            return ex;
        }
        catch (NotFoundException ex)
        {
            return ex;
        }

    }

    private static bool CheckCurrencyMatch(Payment payment, EcocashAccounts ecocashAccount)
    {
        if (payment.PaymentTypeId == (int)PaymentTypes.USD) return ecocashAccount == EcocashAccounts.FCAAccount;
        if (payment.PaymentTypeId == (int)PaymentTypes.USDUtility) return ecocashAccount == EcocashAccounts.FCAAccount;
        return ecocashAccount != EcocashAccounts.FCAAccount;
    }



    public async Task SendNotificationSMS(CompleteEcocashCommand request, BankTrx banktrx, Access access, Account account, Payment payment)
    {
        var smsText = request.Item.GetSmsText(banktrx, account, payment, context);
        SMS sms = banktrx.GetSms(access, smsText);
        var smsResult = await context.SMSs.AddAsync(sms);
        if (smsResult.IsT1) CommandHelper.ReturnDbException(smsResult.AsT1, logger);

    }

    private async Task UpdateBankTrxWithDetails(BankTrx banktrx, Payment payment, OneOf<int, HotDbException> paymentResponse, Transaction item)
    {
        payment.PaymentId = paymentResponse.AsT0;
        banktrx.BankRef = item.ecocashReference;
        banktrx.PaymentID = payment.PaymentId;
        banktrx.BankTrxStateID = (byte)BankTransactionStates.Success;
        var bankTrxUpdate = await context.BankTrxs.UpdateAsync(banktrx);
        if (bankTrxUpdate.IsT1) CommandHelper.ReturnDbException(bankTrxUpdate.AsT1, logger);
    }

    private async Task<Account> GetAccount(Access access)
    {
        var accountResponse = await context.Accounts.GetAsync((int)access.AccountId);
        if (accountResponse.IsT1) throw CommandHelper.ReturnDbException(accountResponse.AsT1, logger);
        Account account = accountResponse.AsT0;
        return account;
    }

    private async Task<Access> GetAccess(BankTrx banktrx)
    {
        var accessId = banktrx.GetAccessId();

        var accessResponse = (accessId == 0)
            ? await context.Accesss.SelectCodeAsync(banktrx.Identifier)
            : await context.Accesss.GetAsync((int)accessId);
        if (accessResponse.IsT1) throw CommandHelper.ReturnDbException(accessResponse.AsT1, logger);
        Access access = accessResponse.AsT0;
        return access;
    }

    private async Task ChangeBankTrxStateToProcessing(BankTrx banktrx)
    {
        banktrx.BankTrxStateID = (byte)BankTransactionStates.ToBeAllocated;
        _ = await context.BankTrxs.UpdateAsync(banktrx);
    }

    private async Task<CompleteBankTrxResult> UpdateTransactiontoFailed(BankTrx bankTrx, Transaction request)
    {
        bankTrx.BankTrxStateID = (byte)BankTransactionStates.Failed;
        bankTrx.BankRef = request.ecocashReference ?? "";
        var bankTrxUpdate = await context.BankTrxs.UpdateAsync(bankTrx);
        return bankTrxUpdate.IsT0
            ? new CompleteBankTrxResult(true, bankTrx, null)
            : new CompleteBankTrxResult(false, bankTrxUpdate.AsT1.Message);
    }

    private async Task<BankTrx> GetBankTrx(CompleteEcocashCommand request)
    {
        long BankTrxId = request.Item.clientCorrelator.Parse<long>();
        var banktrxResponse = await context.BankTrxs.GetAsync((int)BankTrxId);
        if (banktrxResponse.IsT1) throw CommandHelper.ReturnDbException(banktrxResponse.AsT1, logger);
        BankTrx banktrx = banktrxResponse.AsT0;
        if (banktrx == null) throw new NotFoundException("", "");
        return banktrx;
    }
    private static bool HasExpired(string transactionStatus, BankTrx banktrx)
    {
        double transactionAge = (DateTime.Now - banktrx.TrxDate).TotalMinutes;
        if (transactionStatus == "FAILED" && transactionAge > 5) return true;
        if (transactionStatus == "EXPIRED") return true;
        return false;
    }


}

