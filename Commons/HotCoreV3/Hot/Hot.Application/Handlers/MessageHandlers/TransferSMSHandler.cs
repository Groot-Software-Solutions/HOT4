namespace Hot.Application.Handlers.MessageHandlers
{
    public class TransferSMSHandler : BaseSMSMessageHandler, ISMSMessageHandler
    {
        public new HotTypes HotType { get; } = HotTypes.Transfer;

        public TransferSMSHandler(ILogger logger, IDbContext context) : base(logger, context)
        {
        }
        public override async Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            var accessResult = await context.Accesss.SelectCodeAsync(sms.Mobile);
            if (accessResult.IsT1) { return null; }
            var access = accessResult.AsT0;

            var accountResult = await context.Accounts.GetAsync((int)access.AccountId);
            if (accountResult.IsT1) { return null; }
            var account = accountResult.AsT0;

            var sendToAccessResult = await context.Accesss.SelectCodeAsync(sms.SMSText.Split(" ")[2]);
            if (sendToAccessResult.ResultOrNull() == null) { return HelpfailedTransferMessages(sms); }
            var sendToAccess = sendToAccessResult.AsT0;

            var SendToAccountResult = await context.Accounts.GetAsync((int)sendToAccess.AccountId);  
            if(sendToAccessResult.ResultOrNull() == null) { return null; }
            var sendToAccount = SendToAccountResult.AsT0;    

            var Amount = Decimal.Parse(sms.SMSText.Split(' ')[1]);
            Payment paymentFrom = await ProcessFromPayment(access, Amount);
            Payment paymentTo = await ProcessToPayment(access, Amount, paymentFrom);

            await ProcessTransfer(sms, Amount, paymentFrom, paymentTo);
            var templates =new  List<Template>();
            var templateSuccessResult = await context.Templates.GetAsync((int)Templates.SuccessfulTransferReceiver);
            if (templateSuccessResult.IsT1) { return null; }
            var successTemplate = templateSuccessResult.AsT0
                .SetBalance(account.Balance)
                .SetSaleValue(account.SaleValue)
                .SetRefence(paymentFrom.Reference);
            templates.Add(successTemplate); 

            var templateSuccessSenderResult = await context.Templates.GetAsync((int)Templates.SuccessfulTransferSender);
            if (templateSuccessSenderResult.IsT1) { return null; }
            var successSenderTemplate = templateSuccessSenderResult.AsT0
                .SetMobile(sms.SMSText.Split(' ')[2])
                .SetBalance(sendToAccount.Balance)
                .SetAmount(Amount);
            templates.Add(successSenderTemplate);

            var replies = CreateNewSMSfromTemplate(sms.Mobile, templates, sms.SMSID);
            return replies;

        }

        private async Task ProcessTransfer(SMS sms, decimal Amount, Payment paymentFrom, Payment paymentTo)
        {
            Transfer transfer = new()
            {
                Amount = Amount,
                PaymentId_From = paymentFrom.PaymentId,
                PaymentId_To = paymentTo.PaymentId,
                TransferDate = DateTime.Now,
                SmsId = sms.SMSID

            };
            transfer.ChannelId = (int)Channels.Sms;
            await context.Transfers.AddAsync(transfer);
        }

        private async Task<Payment> ProcessToPayment(Access access, decimal Amount, Payment paymentFrom)
        {
            Payment paymentTo = new()
            {
                Reference = paymentFrom.Reference,
                AccountId = access.AccountId,
                Amount = Amount,
                PaymentDate = paymentFrom.PaymentDate,
                LastUser = paymentFrom.LastUser,
                PaymentTypeId = paymentFrom.PaymentTypeId,
                PaymentSourceId = paymentFrom.PaymentSourceId
            };
            await context.Payments.AddAsync(paymentTo);
            return paymentTo;
        }

        private async Task<Payment> ProcessFromPayment(Access access, decimal Amount)
        {
            Payment paymentFrom = new()
            {
                Reference = "Transfer, $" + Amount.ToString("#,##0.00") + " from " + access.AccessCode + " to " + access.AccessCode + ", " + DateTime.Now,
                AccountId = access.AccountId,
                Amount = (0 - Amount),
                PaymentDate = DateTime.Now,
                LastUser = "HOT Service"
            };
            paymentFrom.PaymentTypeId = (int)PaymentTypes.HOTTransfer;
            paymentFrom.PaymentSourceId = (int)PaymentSources.HOTDealer;
            await context.Payments.AddAsync(paymentFrom);
            return paymentFrom;
        }

        private List<SMS>? HelpfailedTransferMessages(SMS sms)
        {
            var handler = new HelpSMSHandler(logger, context);
            sms.SMSText = "help FailedTran";
            return handler.HandleSMS(sms);
        }
    }
}
