using System.Data;

namespace Hot.Application.Actions.AccountActions;
public record TransferWalletAirtimeBalanceCommand(string FromAccessCode, string ToAccessCode, decimal Amount) : IRequest<OneOf<TransferResult, AppException>>;
public class TransferWalletAirtimeBalanceCommandHandler : IRequestHandler<TransferWalletAirtimeBalanceCommand, OneOf<TransferResult, AppException>>
{ 
    private readonly IDbContext context; 

    public TransferWalletAirtimeBalanceCommandHandler(IDbContext context)
    {
        this.context = context; 
    }
     
    public async Task<OneOf<TransferResult, AppException>> Handle(TransferWalletAirtimeBalanceCommand request, CancellationToken cancellationToken)
    {

        var DestinationAccessResponse = await context.Accesss.SelectCodeAsync(request.ToAccessCode);
        if (DestinationAccessResponse.IsT1) return await TransferFailedError(request);
        Access DestinationAccess = DestinationAccessResponse.AsT0;

        var SourceAccessResponse = await context.Accesss.SelectCodeAsync(request.FromAccessCode);
        if (SourceAccessResponse.IsT1) return await TransferFailedError(request);
        Access SourceAccess = SourceAccessResponse.AsT0;

        var SourceAccountResponse = await context.Accounts.GetAsync((int)SourceAccess.AccountId);
        if (SourceAccountResponse.IsT1) return await TransferFailedError(request);
        var SourceAccount = SourceAccountResponse.AsT0;

        if (SourceAccount.Balance < request.Amount) return await InsuffucientBalanceError(request, SourceAccount);

        var dbtransaction = await context.BeginTransactionAsync();
        if (dbtransaction is null) return ExceptionResult(new() { TemplateText = "Data Connection error" });

        //Set Transaction - From
        Payment paymentFrom = GetPaymentFrom(request, SourceAccess);

        var savePaymentFromResult = (await context.Payments.AddAsync(paymentFrom, dbtransaction.Item1, dbtransaction.Item2));
        if (savePaymentFromResult.IsT1) return await TransferFailedError(request);
        paymentFrom.PaymentId = savePaymentFromResult.AsT0;

        //Set Transaction - From
        Payment paymentTo = GetPaymentTo(request, DestinationAccess, paymentFrom);

        var savePaymentToResult = (await context.Payments.AddAsync(paymentTo, dbtransaction.Item1, dbtransaction.Item2));
        if (savePaymentToResult.IsT1) return await TransferFailedError(request, dbtransaction.Item2);
        paymentTo.PaymentId = savePaymentToResult.AsT0;

        //Set Transfer
        Transfer transfer = GetTransferItem(request, paymentFrom, paymentTo);
        var transferResult = (await context.Transfers.AddAsync(transfer, dbtransaction.Item1, dbtransaction.Item2));
        if (transferResult.IsT1) return await TransferFailedError(request, dbtransaction.Item2);
        transfer.TransferId = transferResult.AsT0;


        if (!context.CompleteTransaction(dbtransaction.Item2)) return await TransferFailedError(request, dbtransaction.Item2);


        var replies = new List<SMS>();

        SourceAccountResponse = await context.Accounts.GetAsync((int)SourceAccess.AccountId);
        if (SourceAccountResponse.IsT0) SourceAccount = SourceAccountResponse.AsT0;
        Template replyFrom = (await GetTemplate(Templates.SuccessfulTransferSender))
            .SetAmount(request.Amount)
            .SetBalance(SourceAccount.Balance)
            .SetMobile(DestinationAccess.AccessCode);
         if (SourceAccess.ChannelID is (byte)Channels.Sms or (byte)Channels.USSD) replies.Add(replyFrom.ToSMS(SourceAccess.AccessCode));

        
        Account DestinationAccount = new();
        var DestinationAccountResponse = await context.Accounts.GetAsync((int)DestinationAccess.AccountId);
        if (DestinationAccountResponse.IsT0) DestinationAccount = DestinationAccountResponse.AsT0;
        Template replyTo = (await GetTemplate(Templates.SuccessfulTransferReceiver))
            .SetRefence(paymentTo.Reference)
            .SetBalance(DestinationAccount.Balance)
            .SetSaleValue(DestinationAccount.SaleValue); 
        if (DestinationAccess.ChannelID is (byte)Channels.Sms or (byte)Channels.USSD) replies.Add(replyTo.ToSMS(DestinationAccess.AccessCode));

        replies.ForEach(async reply =>
        {
            await context.SMSs.AddAsync(reply);
        });

        return GetTransferResultItem(request, DestinationAccess, SourceAccess, SourceAccount, transfer, replyFrom, DestinationAccount);
    }

    private static TransferResult GetTransferResultItem(TransferWalletAirtimeBalanceCommand request, Access DestinationAccess, Access SourceAccess, Account SourceAccount, Transfer transfer, Template replyFrom, Account DestinationAccount)
    {
        return new TransferResult()
        {
            Amount = request.Amount,
            FromAccountId = (int)SourceAccess.AccountId,
            ToAccountId = (int)DestinationAccess.AccountId,
            TargetAccountName = DestinationAccount.AccountName,
            ReplyCode = ReplyCode.Success,
            SourceAccountName = SourceAccount.AccountName,
            WalletBalance = SourceAccount.Balance,
            TransferId = (int)transfer.TransferId,
            Message = replyFrom.TemplateText,
        };
    }

    private static Transfer GetTransferItem(TransferWalletAirtimeBalanceCommand request, Payment paymentFrom, Payment paymentTo)
    {
        return new()
        {
            Amount = request.Amount,
            ChannelId = (byte)Channels.Web,
            PaymentId_From = paymentFrom.PaymentId,
            PaymentId_To = paymentTo.PaymentId,
            TransferDate = DateTime.Now,
            SmsId = -1
        };
    }

    private static Payment GetPaymentTo(TransferWalletAirtimeBalanceCommand request, Access DestinationAccess, Payment paymentFrom)
    {
        return new()
        {
            Reference = paymentFrom.Reference,
            AccountId = DestinationAccess.AccountId,
            Amount = request.Amount,
            PaymentDate = paymentFrom.PaymentDate,
            PaymentTypeId = paymentFrom.PaymentTypeId,
            PaymentSourceId = paymentFrom.PaymentSourceId,
            LastUser = "Hot.Web.Api"
        };
    }

    private static Payment GetPaymentFrom(TransferWalletAirtimeBalanceCommand request, Access SourceAccess)
    {
        return new()
        {
            Reference = $"Transfer, ${request.Amount:N0} from {request.FromAccessCode} to {request.ToAccessCode}, {DateTime.Now:dd MMM yyyy HHHH:mm}",
            AccountId = SourceAccess.AccountId,
            Amount = request.Amount * -1,
            PaymentDate = DateTime.Now,
            PaymentTypeId = 6,
            PaymentSourceId = 6,
            LastUser = "Hot.Web.Api"
        };
    }

    private async Task<TransferResult> InsuffucientBalanceError(TransferWalletAirtimeBalanceCommand request, Account SourceAccount)
    {
        Template reply = await GetTemplate(Templates.FailedTransferBalance);
        reply.TemplateText = reply.TemplateText.Replace("%AMOUNT%", request.Amount.ToString("N2"));
        reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
        reply.TemplateText = reply.TemplateText.Replace("%BALANCE%", SourceAccount.Balance.ToString("N2"));
        return FailedTransferResult(reply);
    }

    private async Task<AppException> TransferFailedError(TransferWalletAirtimeBalanceCommand request, IDbTransaction? transaction = null)
    {
        if (transaction is not null) context.RollbackTransaction(transaction);
        Template reply = await GetTemplate(Templates.FailedTransferMobile);
        reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
        return ExceptionResult(reply);
    } 

    private static TransferResult FailedTransferResult(Template reply)
    {
        return new TransferResult()
        {
            Amount = 0,
            FromAccountId = 0,
            ToAccountId = 0,
            TargetAccountName = "",
            ReplyCode = ReplyCode.FailedTransfer,
            SourceAccountName = "",
            WalletBalance = 0,
            TransferId = 0,
            Message = reply.TemplateText,
        };
    }
      
    private static AppException ExceptionResult(Template reply)
    {
        return new AppException("Transfer Error", reply.TemplateText);
    }

    private async Task<Template> GetTemplate(Templates templateId)
    {
        var response = await context.Templates.GetAsync((int)templateId);
        if (response.IsT0) return response.AsT0;
        return new();
    }

}
