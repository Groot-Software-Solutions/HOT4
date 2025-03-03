using Hot.Application.Handlers.MessageHandlers.HelpHandlers;

namespace Hot.Application.Common.Interfaces;
public interface IHelpMessageHandlerFactoryService
{
    IHelpMessageHandler GetHelpHandler(string Name);
    bool HasHandler(string Name);
    string Identify(string sms);
}
