using Domain.DataModels;
using Domain.Interfaces;
using System;
using System.Threading.Tasks; 
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.AspNet.Common;
using Twilio.TwiML.Fax;
using Domain.Entities;

namespace Infrastructure.Services
{
    public class TwilioService : IWhatsAppService 
    {
        readonly IConfigHelper configHelper;

        public TwilioService(IConfigHelper configHelper)
        {
            this.configHelper = configHelper;
        }

        public Message GetMessage<T>(T message)
        {
            var result = new Message();
            if (message.GetType().Name == "SmsRequest")
            {
                var receivedMessage = (SmsRequest)Convert.ChangeType(message, typeof(SmsRequest));
                result.Text = receivedMessage.Body;
                result.Date = DateTime.Now;
                result.PhoneNumber = (receivedMessage.From ?? "")
                    .Replace("whatsapp:","", StringComparison.OrdinalIgnoreCase)
                    .Replace("+263","0");
                result.ConversationId = receivedMessage.SmsSid;
                result.Type = Domain.Enum.MessageType.Text;
            }
            return result;
        }

        public async Task<bool> ReplyAsync(Message message, WhatsAppTemplate reply)
        {
            var accountSid = configHelper.GetVal<string>("TwilioAccountSid");
            var authToken = configHelper.GetVal<string>("TwilioAuthToken");
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                new PhoneNumber($"whatsapp:{NumberFormat(message.PhoneNumber)}"));
            messageOptions.From = new PhoneNumber("whatsapp:+14155238886");
            messageOptions.Body = message.Text;

            var result = await MessageResource.CreateAsync(messageOptions);
            Console.WriteLine(result.Body);
            return (result.Status ==  MessageResource.StatusEnum.Queued);
        }

        public string NumberFormat(string mobile)
        {
            var result = mobile.TrimStart('0');
            
            if (result.StartsWith("+") ==false) result = $"+263{result}";

            return result; 
        }

    }
}
