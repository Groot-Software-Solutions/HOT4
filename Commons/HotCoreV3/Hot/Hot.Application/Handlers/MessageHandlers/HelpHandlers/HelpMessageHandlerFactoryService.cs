namespace Hot.Application.Handlers.MessageHandlers.HelpHandlers
{
    public class HelpMessageHandlerFactoryService : IHelpMessageHandlerFactoryService
    {
        private readonly IReadOnlyDictionary<List<string>, IHelpMessageHandler> ListOfHelpHandlers;

        public HelpMessageHandlerFactoryService(ILogger logger, IDbContext context)
        {
            var helpHandlerType = typeof(ISMSMessageHandler);
            ListOfHelpHandlers = helpHandlerType.Assembly.ExportedTypes
                .Where(x => x.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(x =>
                {
                    return Activator.CreateInstance(x, logger, context);
                })
                .Cast<IHelpMessageHandler>()
                .ToDictionary(x => x.Name, x => x);
        }


        public bool HasHandler(string Name)
        {
            var listofkeys = ListOfHelpHandlers.Keys;
            foreach (var key in listofkeys)
            {
                if (key.Any(k => k == Name)) return true;
            }
            return false;
        }
        public IHelpMessageHandler GetHelpHandler(string Name)
        {
            var listofkeys = ListOfHelpHandlers.Keys;
            foreach (var key in listofkeys)
            {
                if (key.Any(k => k == Name)) return ListOfHelpHandlers[key];
            }
            return DefaultHandler();
        }

        public string Identify(string sms)
        {
            string[] SplitSms = sms.Trim().Split(" ");
            string Query = SplitSms.Length > 1 ? SplitSms[1] : "";
            return HasHandler(Query) ? Query : "";
        }

        private IHelpMessageHandler DefaultHandler()
        {
            var DefaultHandlerName = new List<string> { "" };
            return ListOfHelpHandlers[DefaultHandlerName];
        }
    }
}
