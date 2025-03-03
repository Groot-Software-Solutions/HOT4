namespace Hot.Application.Actions;
public record HandleSMSCommand(SMS Sms) : IRequest<bool>;
        public class HandleSMSCommandHandler : IRequestHandler<HandleSMSCommand, bool>
        { 
            private readonly ILogger<HandleSMSCommandHandler> logger;
            private readonly IHotTypeIdentifier identifier;
            private readonly IMessageHandlerFactory messageHandlerFactory;

            public HandleSMSCommandHandler(ILogger<HandleSMSCommandHandler> logger, IHotTypeIdentifier identifier, IMessageHandlerFactory messageHandlerFactory)
            {
                this.logger = logger;
                this.identifier = identifier;
                this.messageHandlerFactory = messageHandlerFactory;
            }

            public async Task<bool> Handle(HandleSMSCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    HotTypes type = identifier.Identify(request.Sms.SMSText);
                    ISMSMessageHandler handler = messageHandlerFactory.GetHandlerByType(type);
                    await handler.HandleSMSAsync(request.Sms);
                    return true;
                }
                catch (Exception error)
                {
                    LogException("HandleSMS", error);
                    return false;
                }
            }
            
            private void LogException(string logMethod, Exception exception)
            {
                logger.LogError(exception, $"Handle SMS : Handle - { logMethod}");
            }
        }
