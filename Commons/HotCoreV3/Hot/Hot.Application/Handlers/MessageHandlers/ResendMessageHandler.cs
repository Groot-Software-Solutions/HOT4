namespace Hot.Application.Handlers.MessageHandlers
{
    public class ResendMessageHandler : BaseSMSMessageHandler, ISMSMessageHandler
    {
        public new HotTypes HotType { get; } = HotTypes.Answer;

        public ResendMessageHandler(ILogger logger, IDbContext context) : base(logger, context)
        {
        }
        public override async Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            var replies = new List<SMS>();
            string RechargeMobile = "";
            if (sms.SMSText.Split(" ").Length > 1)
            {
                 RechargeMobile = sms.SMSText.Split(" ", 2).ToString();
            }
            sms.State.StateID =(int)States.Success;
            //var smsResult = await context.SMSs.ResendAsync(sms.Mobile, RechargeMobile);
            replies.Add(sms);
            return replies; // revisit
            
        }
    }
}
