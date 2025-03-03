using Hot.Application.Common.Interfaces;
using Hot.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Hot.Infrastructure.FactoryServices
{
    public class MessageHandlerFactory : IMessageHandlerFactory
    {
        public readonly IReadOnlyDictionary<HotTypes, ISMSMessageHandler> messageHandlers;

        public MessageHandlerFactory(ILogger<MessageHandlerFactory> logger, IDbContext context)
        {
            var messageHandlerType = typeof(ISMSMessageHandler);
            var messageHandlers = messageHandlerType.Assembly.ExportedTypes
                .Where(x => messageHandlerType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x =>
                { 
                    return Activator.CreateInstance(x, logger, context);
                })
                .Cast<ISMSMessageHandler>()
                .ToDictionary(x => x.HotType, x => x);
        }

        public ISMSMessageHandler GetHandlerByType(HotTypes type)
        {
            var handler = messageHandlers.GetValueOrDefault(type);
            return handler ?? DefaultHandler();
        }

        private ISMSMessageHandler DefaultHandler()
        {
            return messageHandlers[HotTypes.Unknown];
        }
    }
}
