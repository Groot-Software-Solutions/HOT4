using Domain.DataModels;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class MessageProccessor : IMessageProccessor
    {
        readonly IRechargeHelper rechargeService;
        readonly IDealerService dealerService;
        readonly ITemplateHelper templateHelper;
        readonly IDbContext dbContext;
        readonly ILogger logger;
        readonly IConfigHelper config;

        public MessageProccessor(IRechargeHelper rechargeService, IDealerService dealerService, ITemplateHelper templateHelper, IDbContext dbContext, ILogger logger, IConfigHelper config)
        {
            this.rechargeService = rechargeService;
            this.dealerService = dealerService;
            this.templateHelper = templateHelper;
            this.dbContext = dbContext;
            this.logger = logger;
            this.config = config;
        }

        public async Task<WhatsAppTemplate> ProcessMessage(WhatsAppMessage message)
        {
            var reply = GetUnknownMessageTemplate();
            switch (message.TypeId)
            {
                case RequestType.Airtime:
                    reply = await ProcessAirtime(message);
                    break;
                case RequestType.PinReset:
                    reply = await ProcessPinReset(message);
                    break;
                case RequestType.Greeting:
                    reply = await ProcessTemplateReply(message, TemplateCode.GreetingReply);
                    break;
                case RequestType.SigningOff:
                    reply = await ProcessTemplateReply(message, TemplateCode.SigningoffReply);
                    break;
                case RequestType.HelpRegistration:
                    reply = await ProcessTemplateReply(message, TemplateCode.Registration);
                    break;
                case RequestType.HelpRecharge:
                    reply = await ProcessTemplateReply(message, TemplateCode.HelpRecharge);
                    break;
                case RequestType.HelpEcocash:
                    reply = await ProcessTemplateReply(message, TemplateCode.HelpEcocash);
                    break;
                case RequestType.HelpBank:
                    reply = await ProcessTemplateReply(message, TemplateCode.HelpBank);
                    break;
                case RequestType.HelpPinReset:
                    reply = await ProcessTemplateReply(message, TemplateCode.HelpPinReset);
                    break;
                default:
                    break;
            }
            return reply;
        }

        private async Task<WhatsAppTemplate> ProcessTemplateReply(WhatsAppMessage message, TemplateCode template)
        {
            var reply = await templateHelper.LoadTemplateAsync(template);
            if (reply != null)
            {
                message.StateId = State.Pending;
            } 
            await LogData($"{template} Reply", message);
            _ = await dbContext.WhatsAppMessage_Save(message);
            return reply;
        }
       
        private WhatsAppTemplate GetUnknownMessageTemplate()
        {
            return new WhatsAppTemplate()
            {
                Type = MessageType.Text
                ,
                Text = "Unknown Message Format"
            };
        }

        private async Task LogData(string Method, object data)
        {
            _ = await logger.LogData(new LogItem()
            {
                Module = "MessageProcessor"
                ,
                Method = Method
                ,
                Data = System.Text.Json.JsonSerializer.Serialize(data)
            });
        }

        public SelfTopUp GetSelfTopUp(string message)
        {
            var fields = Regex.Matches(message, config.GetVal<string>("AirtimeFormat"), RegexOptions.IgnoreCase).ToList();
            var TargetMobile = fields.Select(f => f.Groups["TargetMobile"].Value).ToList()[0];
            var Amount = fields.Select(f => f.Groups["Amount"].Value).ToList()[0];
            var BillerMobile = fields.Select(f => f.Groups["BillerMobile"].Value).ToList()[0];
          
            var topup = new SelfTopUp
            {
              TargetMobile =TargetMobile,
              Amount=Convert.ToDecimal(Amount),
              BillerMobile=BillerMobile
            };

            return topup;
           
        }

        private async Task<WhatsAppTemplate> ProcessAirtime(WhatsAppMessage message)
        {

            var reply = new WhatsAppTemplate();
            var recharge = GetSelfTopUp(message.Message);
            await LogData("ProcessAirtime", recharge);
            var submitted = await rechargeService.SubmitSelfTopUp(recharge);
            if (submitted)
            {
                reply = await templateHelper.LoadTemplateAsync(TemplateCode.RechargeAttempt);
                if (reply != null)
                {
                    templateHelper.SetTemplateFields(ref reply, recharge);
                    message.StateId = State.Pending;
                }
            }
            await LogData("ProcessAirtime", reply);
            _ = await dbContext.WhatsAppMessage_Save(message);
            return reply;
        }

        private async Task<WhatsAppTemplate> ProcessPinReset(WhatsAppMessage message)
        {
            var pinReset = GetPinReset(message.Message, message.Mobile);
            await LogData("PinReset", pinReset);
            var result = await dealerService.ResetPin(pinReset);
            var submitted = result.Status == State.Success;
            var reply = await templateHelper.LoadTemplateAsync(submitted ? TemplateCode.PinResetSuccessful : TemplateCode.PinResetFailure);
            if (reply != null)
            {
                templateHelper.SetTemplateFields(ref reply, pinReset);
                message.StateId = State.Pending;
            }

            await LogData("PinReset", reply);
            _ = await dbContext.WhatsAppMessage_Save(message);

            if (!submitted) reply.Text = "Unknown";
            return reply;
        }

        public PinReset GetPinReset(string message, string sender)
        {
            var fields = Regex.Matches(message, config.GetVal<string>("PinResetFormat"), RegexOptions.IgnoreCase).ToList();
            var firstname = fields.Select(f => f.Groups["Firstname"].Value).ToList()[0];
            var surname = fields.Select(f => f.Groups["Surname"].Value).ToList()[0];
            var middlename = fields.Select(f => f.Groups["Middlename"].Value).ToList()[0];
            var idnumber = fields.Select(f => f.Groups["IdNumber"].Value).ToList()[0].Replace(" ", "").Trim();
            var targetnumber = LocalNumber(fields.Select(f => f.Groups["TargetNumber"].Value).ToList()[0].Replace(" ","").Trim()) ?? "";

            var pinReset = new PinReset
            {
                Sender = LocalNumber(sender),
                IDNumber = idnumber,
                Names = $"{firstname} {surname} {middlename}".Trim(),
                TargetMobile = targetnumber == "" ? LocalNumber(sender) : targetnumber
            }; 

            return pinReset;
        }

        private string LocalNumber(string number)
        {
            if (number.StartsWith("+263") || number.StartsWith("263")) number = number.Replace("+", "").Replace("263", "0");
            return number;
        }
    }


}
