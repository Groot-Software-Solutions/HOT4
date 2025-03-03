namespace Hot.Application.Common.Interfaces
{
    public interface IMessageHandlerFactory
    {
        public ISMSMessageHandler GetHandlerByType(HotTypes type);
    }
}
