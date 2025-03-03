namespace Hot.Application.Handlers.MessageHandlers
{
    public class SMSMessageHandler
    {
        private readonly IDbContext context;
        private readonly ILogger<SMSMessageHandler> logger;
        private readonly IMessageHandlerFactory messageHandlerFactory;

        public SMSMessageHandler(IDbContext context, ILogger<SMSMessageHandler> logger, IMessageHandlerFactory messageHandlerFactory)
        {
            this.context = context;
            this.logger = logger;
            this.messageHandlerFactory = messageHandlerFactory;
        }

        public async Task HandleSMS(SMS sms)
        {
            var HotType = await IdentifyHotTypeFromSMSAsync(sms); 
            var handler = messageHandlerFactory.GetHandlerByType(HotType);
            var replies = await handler.HandleSMSAsync(sms); 
            var response = await SaveReplies(replies);
            sms.State.StateID = (int)(response ? States.Success : States.Failure);
            await UpdateOriginalSMS(sms);
        }

        private async Task<HotTypes> IdentifyHotTypeFromSMSAsync(SMS sms)
        {
            var TypeCode = sms.SMSText.Split(" ")[0];
            var splitCount = sms.SMSText.Split(" ").Length;
            var HotType = (await context.HotTypes.IndentifyAsync(TypeCode, splitCount)).ResultOrNull();
            if (HotType == -1) return 0;
            return (HotTypes)HotType;
        }

        public async Task<bool> SaveReplies(List<SMS>? replies)
        {
            var response = true;
            if (replies == null) return true;
            foreach (var reply in replies)
            {
                response = await SaveReply(reply) && response;
            };
            return response;
        }

        public async Task<bool> SaveReply(SMS Sms)
        {
            var saveReplyResponse = await context.SMSs.AddAsync(Sms);
            return saveReplyResponse.Match(
                id => {
                    Sms.SMSID = id;
                    return true;
                },
                error =>
                {
                    LogException("SaveSMS", error);
                    return false;
                });
        }

        public async Task UpdateOriginalSMS(SMS sms)
        {
            var updateSMSResponse = await context.SMSs.UpdateAsync(sms);
            updateSMSResponse.Switch(
                saved =>
                {
                    LogDebugInformation("UpdateOriginalSMS",
                        $"Completed Handling SMS ID:{sms.SMSID} Text: {sms.SMSText} Date: {sms.SMSDate.ToString("dd-MMM-yy HH:mm:ss")}");
                },
                error =>
                {
                    LogException("UpdateOriginalSMS", error);
                });
        }

      
        public void LogDebugInformation(string logMethod, string message)
        {
            logger.LogDebug($"SMS Handler Service:{logMethod}{message}");
        }

        public void LogException(string logMethod, Exception exception)
        {
            logger.LogError(exception, $"SMS Handler Service:{logMethod}");
        }


    }
}
