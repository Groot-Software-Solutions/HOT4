using Domain.DataModels;
using Domain.Enum;
using Domain.Interfaces;
using Infrastructure.Services.MaytApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks; 

namespace Infrastructure.Services
{
    public class MaytApiService : IWhatsAppService
    {
        readonly IConfigHelper configHelper;
        readonly IAPIHelper apiHelper;

        public MaytApiService(IConfigHelper helper, IAPIHelper client)
        {
            configHelper = helper;
            apiHelper = client;
            apiHelper.APIName = "WhatsApp";
        }

        public async Task<bool> ReplyAsync(Domain.Entities.Message message, WhatsAppTemplate reply)
        {
            var phoneId = configHelper.GetVal<string>("WhatsAppWebPhoneId");
            var webmethod = $"{phoneId}/sendMessage";
            var messagetosend = new SendMessageModel {
                message = reply.Text
                , to_number = message.PhoneNumber
                , type = reply.Type.ToString().ToLower()
            };
            var result  = await apiHelper.APIPostCall<SendMessageResponseModel, SendMessageModel>(webmethod, messagetosend);
            return result.success;
        }

        public DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public MessageType IdentifyMessageType(string type)
        {
            var result  = (Enum.IsDefined(typeof(MessageType), type)
                ? (MessageType)Enum.Parse(typeof(MessageType), type)
                : MessageType.Unknown
                );
            if (result == MessageType.Unknown && type == "text") result = MessageType.Text;

            return result;
        }

        public Domain.Entities.Message GetMessage<T>(T message)
        {
            var result = new Domain.Entities.Message();
            if (message.GetType().Name == "ReceivedMessage")
            {
                var receivedMessage = (ReceivedMessage)Convert.ChangeType(message, typeof(ReceivedMessage));
                result.Text = receivedMessage.body.message.text;
                result.Date = UnixTimeStampToDateTime(receivedMessage.body.timestamp);
                result.PhoneNumber = receivedMessage.body.user.phone;
                result.ConversationId = receivedMessage.body.conversation;
                result.Type = IdentifyMessageType(receivedMessage.body.message.type);
               
            }
            if (message.GetType().Name == "ReceivedMessageModel")
            {
                var receivedMessage = (ReceivedMessageModel)Convert.ChangeType(message, typeof(ReceivedMessageModel));
                result.Text = receivedMessage.message.text;
                result.Date = UnixTimeStampToDateTime(receivedMessage.timestamp);
                result.PhoneNumber = receivedMessage.user.phone;
                result.ConversationId = receivedMessage.conversation;
                result.Type = IdentifyMessageType(receivedMessage.message.type);

            }
            return result;
        }


    }

}
