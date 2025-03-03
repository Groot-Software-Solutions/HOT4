using System.Data;

namespace Hot.Application.Actions.AccountActions;

public record TransferUSDAirtimeWalletBalanceCommand(string FromAccessCode, string ToAccessCode, decimal Amount) : IRequest<OneOf<TransferResult, AppException>>;
public class TransferUSDAirtimeWalletBalanceCommandHandler : IRequestHandler<TransferUSDAirtimeWalletBalanceCommand, OneOf<TransferResult, AppException>>
{

    private readonly IDbContext context;
    private IDbHelper DbHelper { get; set; }

    public TransferUSDAirtimeWalletBalanceCommandHandler(IDbContext context, IDbHelper dbHelper)
    {
        this.context = context;
        DbHelper = dbHelper;
    }


    public async Task<OneOf<TransferResult, AppException>> Handle(TransferUSDAirtimeWalletBalanceCommand request, CancellationToken cancellationToken)
    { 

        Access DestinationAccess = (await context.Accesss.SelectCodeAsync(request.ToAccessCode)).ResultOrNull();
        if (DestinationAccess == null)
        {
            Template reply = await GetTemplate(Templates.FailedTransferMobile);
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.ToAccessCode);
            return ExceptionResult(reply);
        }

        Access SourceAccess = (await context.Accesss.SelectCodeAsync(request.FromAccessCode)).ResultOrNull();
        if (DestinationAccess == null)
        {
            Template reply = await GetTemplate(Templates.FailedTransferMobile);
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
            return ExceptionResult(reply);
        }

        //Validate amount range
        //if (amount < await con.MinTransfer)
        //{
        //    Template reply = (await context.Templates.GetAsync((int)Templates.FailedTransferMin)).ResultOrNull();
        //    reply.TemplateText = reply.TemplateText.Replace("%AMOUNT%", amount.ToString("N2"));
        //    reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", _Sms.SmsText.Split(' ')[2]);
        //    reply.TemplateText = reply.TemplateText.Replace("%MIN%", _Config.MinTransfer.ToString("N2"));
        //    AddReply(reply);
        //    return null;
        //}

        //Check Balance
        var SourceAccount = (await context.Accounts.GetAsync((int)SourceAccess.AccountId)).ResultOrNull();
        if (SourceAccount == null)
        {
            Template reply = await GetTemplate(Templates.FailedTransferMobile);
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
            return ExceptionResult(reply);
        }
        if (SourceAccount.WalletBalance(WalletTypes.USD) < request.Amount)
        {
            Template reply = await GetTemplate(Templates.FailedTransferBalance);
            reply.TemplateText = reply.TemplateText.Replace("%AMOUNT%", request.Amount.ToString("N2"));
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
            reply.TemplateText = reply.TemplateText.Replace("%BALANCE%", SourceAccount.WalletBalance(WalletTypes.USD).ToString("N2"));
            return FailedTransferResult(reply);
        }

        var transactionresult = await BeginTransaction();
        if (transactionresult.IsT1) return ExceptionResult("Data Connection error");
        var dbtransaction = transactionresult.AsT0;
        //Set Transaction - From
        Payment paymentFrom = new()
        {
            Reference = $"Transfer, ${request.Amount:N0} from {request.FromAccessCode} to {request.ToAccessCode}, {DateTime.Now:dd MMM yyyy HHHH:mm}",
            AccountId = SourceAccess.AccountId,
            Amount = request.Amount * -1,
            PaymentDate = DateTime.Now,
            PaymentTypeId = 17,
            PaymentSourceId = 6,
            LastUser = "Hot.Web.Api"
        };

        var savePaymentFromResult = (await context.Payments.AddAsync(paymentFrom, dbtransaction.Item1, dbtransaction.Item2));
        if (savePaymentFromResult.IsT1)
        {
            Template reply = await GetTemplate(Templates.FailedTransferMobile);
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
            return ExceptionResult(reply);
        }
        paymentFrom.PaymentId = savePaymentFromResult.AsT0;

        //Set Transaction - From
        Payment paymentTo = new()
        {
            Reference = paymentFrom.Reference,
            AccountId = DestinationAccess.AccountId,
            Amount = request.Amount,
            PaymentDate = paymentFrom.PaymentDate,
            PaymentTypeId = paymentFrom.PaymentTypeId,
            PaymentSourceId = paymentFrom.PaymentSourceId,
            LastUser = "Hot.Web.Api"
        };

        var savePaymentToResult = (await context.Payments.AddAsync(paymentTo, dbtransaction.Item1, dbtransaction.Item2));
        if (savePaymentToResult.IsT1)
        {
            Template reply = await GetTemplate(Templates.FailedTransferMobile);
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
            DbHelper.RollBackTransaction(dbtransaction.Item2);
            return ExceptionResult(reply);
        }
        paymentTo.PaymentId = savePaymentToResult.AsT0;

        //Set Transfer
        Transfer transfer = new()
        {
            Amount = request.Amount,
            ChannelId = (byte)Channels.Web,
            PaymentId_From = paymentFrom.PaymentId,
            PaymentId_To = paymentTo.PaymentId,
            TransferDate = DateTime.Now,
            SmsId = -1
        };
        var transferResult = (await context.Transfers.AddAsync(transfer, dbtransaction.Item1, dbtransaction.Item2));
        if (transferResult.IsT1)
        {
            Template reply = await GetTemplate(Templates.FailedTransferMobile);
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
            DbHelper.RollBackTransaction(dbtransaction.Item2);
            return ExceptionResult(reply);
        }
        transfer.TransferId = transferResult.AsT0;
        if (!CompleteTransaction(dbtransaction))
        {
            Template reply = await GetTemplate(Templates.FailedTransferMobile);
            reply.TemplateText = reply.TemplateText.Replace("%MOBILE%", request.FromAccessCode);
            DbHelper.RollBackTransaction(dbtransaction.Item2);
            return ExceptionResult(reply);
        };


        SourceAccount = (await context.Accounts.GetAsync((int)SourceAccess.AccountId)).ResultOrNull();
        var DestinationAccount = (await context.Accounts.GetAsync((int)DestinationAccess.AccountId)).ResultOrNull();
        var replies = new List<SMS>();
        Template replyFrom = await GetTemplate(Templates.SuccessfulTransferSender);
        replyFrom.TemplateText = replyFrom.TemplateText.Replace("%AMOUNT%", request.Amount.ToString("N2"));
        replyFrom.TemplateText = replyFrom.TemplateText.Replace("%BALANCE%", SourceAccount.WalletBalance(WalletTypes.USD).ToString("N2"));
        replyFrom.TemplateText = replyFrom.TemplateText.Replace("%MOBILE%", DestinationAccess.AccessCode);
        if (SourceAccess.ChannelID == (byte)Channels.Sms) replies.Add(GetSMSFromTemplate(replyFrom, SourceAccess.AccessCode));

        //Reply To 
        Template replyTo = await GetTemplate(Templates.SuccessfulTransferReceiver);
        replyTo.TemplateText = replyTo.TemplateText.Replace("%REF%", paymentTo.Reference);
        replyTo.TemplateText = replyTo.TemplateText.Replace("%BALANCE%", DestinationAccount.WalletBalance(WalletTypes.USD).ToString("N2"));
        replyTo.TemplateText = replyTo.TemplateText.Replace("%SALEVALUE%", DestinationAccount.WalletBalance(WalletTypes.USD).ToString("N2"));
        if (DestinationAccess.ChannelID == (byte)Channels.Sms) replies.Add(GetSMSFromTemplate(replyTo, DestinationAccess.AccessCode));

        foreach (var reply in replies)
        {
            await context.SMSs.AddAsync(reply);
        }

        return new TransferResult()
        {
            Amount = request.Amount,
            FromAccountId = (int)SourceAccess.AccountId,
            ToAccountId = (int)DestinationAccount.AccountID,
            TargetAccountName = DestinationAccount.AccountName,
            ReplyCode = ReplyCode.Success,
            SourceAccountName = SourceAccount.AccountName,
            WalletBalance = SourceAccount.Balance,
            TransferId = (int)transfer.TransferId,
            Message = replyFrom.TemplateText,
        };
    }

    private static TransferResult FailedTransferResult(Template reply)
    {
        return new TransferResult()
        {
            Amount =0,
            FromAccountId = 0,
            ToAccountId = 0,
            TargetAccountName = "",
            ReplyCode = ReplyCode.FailedTransfer,
            SourceAccountName = "",
            WalletBalance = 0,
            TransferId =0,
            Message = reply.TemplateText,
        };
    }

    private static SMS GetSMSFromTemplate(Template template, string Mobile)
    {
        return new SMS
        {
            Direction = false,
            Mobile = Mobile,
            Priority = new Priority() { PriorityId = (byte)Priorities.Normal },
            State = new State() { StateID = (byte)States.Pending },
            SMSID_In = (long?)null,
            SMSText = template.TemplateText,
        };
    }

    private static OneOf<TransferResult, AppException> ExceptionResult(Template reply)
    {
        return ExceptionResult(reply.TemplateText);
    }

    private static OneOf<TransferResult, AppException> ExceptionResult(string reply)
    {
        return new AppException("Application Error", reply);
    }

    private async Task<Template> GetTemplate(Templates templateId)
    {
        return (await context.Templates.GetAsync((int)templateId)).ResultOrNull();
    }

    private async Task<OneOf<Tuple<IDbConnection, IDbTransaction>, AppException>> BeginTransaction()
    {
        var result = await DbHelper.BeginTransaction();
        if (result.ResultOrNull() == null)
        {
            return new AppException("DB Error attempting to create transaction", result.AsT1.Message);
        }
        return result.ResultOrNull();
    }
    public bool CompleteTransaction(Tuple<IDbConnection, IDbTransaction> dbtransaction)
    {
        return DbHelper.CommitTransaction(dbtransaction.Item2).ResultOrNull();
    }
}