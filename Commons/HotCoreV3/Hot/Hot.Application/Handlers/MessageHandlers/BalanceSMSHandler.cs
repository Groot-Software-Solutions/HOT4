namespace Hot.Application.Handlers.MessageHandlers
{
    public class BalanceSMSHandler : BaseSMSMessageHandler, ISMSMessageHandler
    {
        public new HotTypes HotType { get; } = HotTypes.Balance;
 
        public BalanceSMSHandler(ILogger logger, IDbContext context) : base(logger, context)
        { 
        }

        public override async Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            var accessResponse = await context.Accesss.SelectCodeAsync(sms.Mobile);
            if (accessResponse.IsT1) return HelpRegisterMessages(sms);
            var access = accessResponse.AsT0;

            var accountResponse = await context.Accounts.GetAsync((int)access.AccountId);
            if (accountResponse.IsT1) return null;
            var account = accountResponse.AsT0;

            var templateresponse = await context.Templates.GetAsync((int)Templates.SuccessfulBalance);
            if (templateresponse.IsT1) return null;
            var template = templateresponse.AsT0
                .SetAmount(account.Balance)
                .SetSaleValue(account.SaleValue); 
            var replies = CreateNewSMSfromTemplate(sms.Mobile, new List<Template>() { template }, sms.SMSID);
            return replies;
        }

        private List<SMS>? HelpRegisterMessages(SMS sms)
        {
            var handler  =  new HelpSMSHandler(logger,context);
            sms.SMSText = "help REGISTER";
            return handler.HandleSMS(sms);
        }
         
    }

    
}
