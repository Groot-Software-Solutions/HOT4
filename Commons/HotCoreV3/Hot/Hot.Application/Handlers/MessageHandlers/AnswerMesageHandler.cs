namespace Hot.Application.Handlers.MessageHandlers
{
    public class AnswerMesageHandler : BaseSMSMessageHandler, ISMSMessageHandler
    {
        public new HotTypes HotType { get; } = HotTypes.Answer;

        public AnswerMesageHandler(ILogger logger, IDbContext context) : base(logger, context)
        {
        }

        public override async Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            var templates = new List<Template>();
            if (sms.SMSText[..3].ToUpper() == "OPT")
            {
                if (sms.SMSText.ToUpper() == "OPT OUT" || sms.SMSText.ToUpper() == "OPTOUT")
                {
                    var templateResult = await context.Templates.GetAsync((int)Templates.AnswerOK);
                    if (templateResult.IsT1) { return null; }
                    var template = templateResult.AsT0;
                    template.TemplateText = "You have chosen to Opt Out of future information or competitions with HOT recharge. Thank you for your business";
                    templates.Add(template);
                }
                else
                {
                    var templateResult = await context.Templates.GetAsync((int)Templates.AnswerOK);
                    if (templateResult.IsT1) { return null; }
                    var template = templateResult.AsT0;
                    template.TemplateText = "You have chosen to Opt In to get future information & competitions with HOT recharge. Thank you from HOT Recharge";
                    templates.Add(template);
                }
            }
            else
            {
                if (sms.SMSText.Trim().Split(" ").Length > 1)
                {
                    var templateResult = await context.Templates.GetAsync((int)Templates.AnswerOK);
                    if (templateResult.IsT1) { return null; }
                    var template = templateResult.AsT0;
                    var message = sms.SMSText.Split(" ", 1).ToString();
                    template.SetMessage(message ?? "");
                    templates.Add(template);
                }
                else
                {
                    var templateResult = await context.Templates.GetAsync((int)Templates.AnswerOK);
                    if (templateResult.IsT1) { return null; }
                    var template = templateResult.AsT0;
                    templates.Add(template);
                }
            }
            var replies = CreateNewSMSfromTemplate(sms.Mobile, templates, sms.SMSID);
            return replies;
        }
    }
}
