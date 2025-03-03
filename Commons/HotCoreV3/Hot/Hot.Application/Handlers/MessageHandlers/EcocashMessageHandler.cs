namespace Hot.Application.Handlers.MessageHandlers
{
    public class EcocashMessageHandler : BaseSMSMessageHandler, ISMSMessageHandler
    {
        public new HotTypes HotType { get; } = HotTypes.EcoCash;

        public EcocashMessageHandler(ILogger logger, IDbContext context) : base(logger, context)
        {
        }

        public override async Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            var templates = new List<Template>();
            //Check if Ecocash is enabled
            //if not enabled 
            var disabledTemplate = await context.Templates.GetAsync((int)Templates.PaymentFailed);
            if(disabledTemplate.ResultOrNull() == null) { return null; }
            var template = disabledTemplate.ResultOrNull();
            template.TemplateText = "EcoCash request Failed: Ecocash platform down, please try again later. HOT Recharge";
            templates.Add(template);

            var accessResult = await context.Accesss.SelectCodeAsync(sms.Mobile);
            if(accessResult.ResultOrNull() == null)
            {
                var templateResult = await context.Templates.GetAsync((int)Templates.FailedNotRegistered);
                if (templateResult.ResultOrNull() == null) { return null; }
                var templateenabled = templateResult.ResultOrNull();
                templates.Add(templateenabled); 
                
            }

            if (sms.SMSText.Split(" ").Length != 2) 
            {
                var failedTemplate = await context.Templates.GetAsync((int)Templates.FailedRechargeFormat);
                if(failedTemplate.ResultOrNull() == null) { return null; }
                var templatefailed = failedTemplate.ResultOrNull();
                templatefailed.TemplateText = "HOT Recharge cannot understand your message:" + sms.SMSText + ", sms ECOCASH AmountWanted - HOT Recharge";
                templates.Add(templatefailed);
            }
            
            if(!(decimal.TryParse(sms.SMSText.Split(" ", 1).ToString(), out decimal amount)))
            {
                var failedTemplate = await context.Templates.GetAsync((int)Templates.FailedRechargeFormat);
                if (failedTemplate.ResultOrNull() == null) { return null; }
                var templatefailed = failedTemplate.ResultOrNull();
                templatefailed.TemplateText = "HOT Recharge cannot understand your message:" + sms.SMSText + ", sms ECOCASH AmountWanted - HOT Recharge";
                templates.Add(templatefailed);
            }
            //Create FundMe object and send to Hot.Ecocash FundWallet
            
            var replies = CreateNewSMSfromTemplate(sms.Mobile, templates, sms.SMSID);
            return replies;
        }
    }
}
