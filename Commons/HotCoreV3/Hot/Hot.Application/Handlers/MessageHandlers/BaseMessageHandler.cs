namespace Hot.Application.Handlers.MessageHandlers
{
    public abstract class BaseSMSMessageHandler : ISMSMessageHandler
    {

        public abstract Task<List<SMS>?> HandleSMSAsync(SMS sms);

        public List<SMS>? HandleSMS(SMS sms)
        {
            return HandleSMSAsync(sms).GetAwaiter().GetResult();
        }

        public HotTypes HotType { get; } = HotTypes.Unknown;
        public string LogClass = "HandleUnknown";

        public readonly ILogger logger;
        public readonly IDbContext context;

        protected BaseSMSMessageHandler(ILogger logger, IDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public static List<SMS> CreateNewSMSfromTemplate(string Mobile, List<Template> replies, long? SMSId = null)
        {
            return replies
                .Select(reply => reply.ToSMS(Mobile, SMSId))
                .ToList();
        }


    }
}
