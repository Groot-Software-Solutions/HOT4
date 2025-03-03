namespace Hot.Application.Handlers.MessageHandlers
{
    public class UnknownSMSHandler : BaseSMSMessageHandler, ISMSMessageHandler
    { 
        public new HotTypes HotType { get; } = HotTypes.Unknown; 
        public UnknownSMSHandler(ILogger logger, IDbContext context) : base(logger, context)
        {
            LogClass = GetType().Name; 
        }

        public async override Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            try
            {
                var templateResult = await context.Templates.GetAsync((int)Templates.UnknownRequest);
                if(templateResult.IsT1) { return null; }
                var template = templateResult.AsT0; 
                var replies = CreateNewSMSfromTemplate(sms.Mobile, new List<Template>() { template }, sms.SMSID);
                return replies;
                 
            }
            catch (Exception ex)
            {
                ex.LogError(logger);
                return null;
            }
        }

        
    }
}
