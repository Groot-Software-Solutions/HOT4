using Hot.Application.Handlers.MessageHandlers.HelpHandlers;

namespace Hot.Application.Handlers.MessageHandlers
{
    public class HelpSMSHandler : BaseSMSMessageHandler, ISMSMessageHandler
    {
        private readonly IHelpMessageHandlerFactoryService helpHandlerFactory;
        public new HotTypes HotType { get; } = HotTypes.Help;

        public HelpSMSHandler(ILogger logger, IDbContext context) : base(logger, context)
        {
            helpHandlerFactory = new HelpMessageHandlerFactoryService(logger, context);
        }

        public override async Task<List<SMS>?> HandleSMSAsync(SMS sms)
        {
            try
            {
                string section = helpHandlerFactory.Identify(sms.SMSText);
                var handler = helpHandlerFactory.GetHelpHandler(section);
                var templates = (await handler.HandleAsync(sms.SMSText)).Match
                    (
                        result => result,
                        error => new List<Template>()
                    );

                var replies = CreateNewSMSfromTemplate(sms.Mobile, templates, sms.SMSID);
                return replies;
            }
            catch (Exception ex)
            {
                logger.LogError("Handle", ex);
                return null;
            }
        }



        //private HelpOptions Identify(string query)
        //{
        //    query = (query.ToUpper() ?? "");
        //    if (query == "BANK" || query == "BANKS") return HelpOptions.Bank;
        //    if (query == "STOCK" || query == "STOCKS") return HelpOptions.Stocks;
        //    if (query == "DISC" || query == "DISCOUNT" || query == "COM" || query == "COMM" || query == "COMMISSION") return HelpOptions.Discount;
        //    if (query == "RECHARGE" || query == "HOT") return HelpOptions.Recharge;
        //    if (query == "REG" || query == "REGISTER") return HelpOptions.Register;
        //    if (query == "ECO" || query == "ECOCASH" || query == "EC0" || query == "EC0CASH") return HelpOptions.Ecocash;
        //    if (query == "PIN" || query == "RESETPIN") return HelpOptions.PinReset;
        //    return HelpOptions.Unknown;
        //}

        //private async Task<List<Template>> GetHelpSMSsAsync(string Query, SMS sms)
        //{
        //    var iList = new List<Template>();
        //    var templates = Identify(Query) switch
        //    {
        //        HelpOptions.Bank => await HelpBank(),
        //        HelpOptions.Discount => await HelpDiscount(sms),
        //        HelpOptions.Stocks => await HelpPinStock(),
        //        HelpOptions.Ecocash => await HelpEcoCash(),
        //        HelpOptions.Recharge => await HelpRecharge(),
        //        HelpOptions.Register => await HelpRegister(),
        //        HelpOptions.PinReset => await HelpPinReset(sms),
        //        _ => await DefaultReply()
        //    };
        //    iList.AddRange(templates); 
        //    return iList; 
        //}

        //private async Task<List<Template>> HelpPinReset(SMS sms)
        //{
        //    if (!ResetPinIfPossible(sms))
        //    {
        //        return new List<Template>() {
        //            await GetTemplate((int)Templates.HelpResetPin)
        //        };
        //    }
        //    return new List<Template>();
        //}

        //private async Task<List<Template>> HelpRegister()
        //{
        //    return new List<Template>() {
        //        await GetTemplate((int)Templates.HelpRegister)
        //    };
        //}

        //private async Task<List<Template>> HelpRecharge()
        //{
        //    return new List<Template>() {
        //        await GetTemplate((int)Templates.HelpRecharge)
        //    };

        //}

        //private async Task<List<Template>> HelpEcoCash()
        //{
        //    return new List<Template>() {
        //        await GetTemplate((int)Templates.HelpEcoCash)
        //    };
        //}

        //private async Task<List<Template>> DefaultReply()
        //{
        //    return new List<Template>(){
        //        await GetTemplate((int)Templates.HelpDefault),
        //        await GetTemplate((int)Templates.HelpRecharge)
        //    };
        //}

        //private async Task<List<Template>> HelpBank()
        //{
        //    return new List<Template>() {
        //        await GetTemplate((int)Templates.HelpBank)
        //    };
        //}

        //private Task<List<Template>> HelpPinStock()
        //{
        //    throw new NotImplementedException();
        //}

        //private Task<List<Template>> HelpDiscount(SMS sms)
        //{
        //    throw new NotImplementedException();
        //}

        //private bool ResetPinIfPossible(SMS sms)
        //{
        //    throw new NotImplementedException();
        //}


    }
}
