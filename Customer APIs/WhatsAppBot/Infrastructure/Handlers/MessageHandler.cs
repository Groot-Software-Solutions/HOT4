using Domain.Interfaces;
using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Infrastructure.Services;
using Domain.Enum;
using System.Net.Mime; 
using Domain.Entities;

namespace Infrastructure.Handlers
{
    public class MessageHandler : IMessageHandler
    {
        private readonly string AirtimeFormat;
        private readonly string PinResetFormat;
        private readonly string HelpMessageFormat;
        private readonly string HelpEcoCashFormat;
        private readonly string HelpBankingFormat;
        private readonly string HelpBalanceFormat;
        private readonly string HelpRegisterFormat;
        private readonly string GreetingFormat;
        private readonly string SignoffFormat;
        private readonly string HelpRechargeFormat;
        private readonly string HelpPinFormat;


        private readonly IDbContext _context;
        private readonly IConfigHelper _configHelper;
        private readonly ILogger _logger;

        public MessageHandler(IDbContext context, IConfigHelper configHelper, ILogger logger)
        {
            _context = context;
            _configHelper = configHelper; 
            _logger = logger;

            AirtimeFormat = _configHelper.GetVal<string>("AirtimeFormat");
            PinResetFormat = _configHelper.GetVal<string>("PinResetFormat");
            HelpMessageFormat = _configHelper.GetVal<string>("HelpMessageFormat");
            HelpEcoCashFormat = _configHelper.GetVal<string>("HelpEcoCashFormat");
            HelpBankingFormat = _configHelper.GetVal<string>("HelpBankingFormat");
            HelpBalanceFormat = _configHelper.GetVal<string>("HelpBalanceFormat");
            HelpRegisterFormat = _configHelper.GetVal<string>("HelpRegisterFormat");
            GreetingFormat = _configHelper.GetVal<string>("GreetingFormat");
            SignoffFormat = _configHelper.GetVal<string>("SigningOffFormat");
            HelpRechargeFormat = _configHelper.GetVal<string>("HelpRechargeFormat");
            HelpPinFormat = _configHelper.GetVal<string>("HelpPinFormat");
        }

        public async Task<WhatsAppMessage> HanldeMessage(Message message)
        {
            WhatsAppMessage result = new WhatsAppMessage();
            switch (message.Type)
            {
                case MessageType.Text:
                    result = IdentifyMessage(message);
                    result.Id = await SaveWhatsAppMessage(result);
                    break;
                default:
                    await LogRawMessage(message);
                    break;
            }
            return result;
        }

        private WhatsAppMessage IdentifyMessage(Message message)
        {
            return new WhatsAppMessage()
            {
                Mobile = message.PhoneNumber
                , ConversationId = message.ConversationId
                , Message = message.Text
                , MessageDate = message.Date
                , TypeId = IdentifyRequest(message.Text)
                , StateId = State.New
            };
        }

        public RequestType IdentifyRequest(string message)
        {
            // Order Matters Less specific at bottom

            if (Regex.Match(message, AirtimeFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.Airtime;
            if (Regex.Match(message, PinResetFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.PinReset;


            if (Regex.Match(message, HelpEcoCashFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.HelpEcocash;
            if (Regex.Match(message, HelpBalanceFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.HelpBalance;
            if (Regex.Match(message, HelpBankingFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.HelpBank;
            if (Regex.Match(message, HelpRegisterFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.HelpRegistration;
            if (Regex.Match(message, HelpPinFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.HelpPinReset;

            if (Regex.Match(message, GreetingFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.Greeting; 

            if (Regex.Match(message, SignoffFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.SigningOff;
            if (Regex.Match(message, HelpRechargeFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.HelpRecharge;



            if (Regex.Match(message, HelpMessageFormat, RegexOptions.IgnoreCase).Success)
                return RequestType.Help;
            return RequestType.Unknown;
        }
        
      
        private async Task LogRawMessage(Message message)
        {
            await _logger.LogWebItem(new WebAPILog()
            {
                Module = "ReceiveHandler",
                RequestTime = DateTime.Now,
                Method = "POST",
                Body = System.Text.Json.JsonSerializer.Serialize(message)
            });
        }

       
        private async Task<int> SaveWhatsAppMessage(WhatsAppMessage message)
        { 
           return await _context.WhatsAppMessage_Save(message); 
        }

     
    }
}
