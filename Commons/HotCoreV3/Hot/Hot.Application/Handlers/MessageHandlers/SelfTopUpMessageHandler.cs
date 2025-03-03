namespace Hot.Application.Handlers.MessageHandlers
{
    public class SelfTopUpMessageHandler : BaseSMSMessageHandler, ISMSMessageHandler
    {
        public new HotTypes HotType { get; } = HotTypes.EcoChargeOther;

        public SelfTopUpMessageHandler(ILogger logger, IDbContext context) : base(logger, context)
        {
        }

        public override async Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            var templates = new List<Template>();
            //check if Ecocash is enabled
            //if not
            var templateResult = await context.Templates.GetAsync((int)Templates.PaymentFailed);
            if (templateResult.ResultOrNull() == null) { return null; }
            var template = templateResult.ResultOrNull();
            template.TemplateText = "EcoCash request Failed: Ecocash platform down, please try again later. HOT Recharge";
            templates.Add(template);
            //Get Mobile
            var mobile = sms.SMSText.Split(" ").Length == 3 ? sms.SMSText.Split(" ", 2).ToString() : sms.Mobile;
            //check if Telecel is enabled 
            if ((mobile ?? "").StartsWith("073"))
            {
                //Is Telecel disabled
                //If yes
                var disabledTemplate = await context.Templates.GetAsync((int)Templates.FailedRechargeVASDisabled);
                if (disabledTemplate.ResultOrNull() == null) { return null; }
                var templatee = disabledTemplate.ResultOrNull();
                templatee.TemplateText = "Please note Telecel is down and currently unable to restore their systems." +
                    "Please do not try to process transactions until further notice HOT Recharge";
                templates.Add(templatee);
            }

            var accessResult = await context.Accesss.SelectCodeAsync(sms.Mobile);
            if (accessResult.ResultOrNull() == null)
            {
                //SelfTopUp from hot.Ecocash
            }
            var replies = CreateNewSMSfromTemplate(sms.Mobile, templates, sms.SMSID);
            return replies;
        }
    }
}
