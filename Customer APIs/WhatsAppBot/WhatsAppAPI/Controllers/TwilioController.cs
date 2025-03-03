using Domain.DataModels;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using System.Threading.Tasks;
using Infrastructure.Handlers;
using System.Data.SqlClient;

namespace WebAPI.Controllers
{
    [Route("api/twilio")]
    public class TwilioServiceController : TwilioController
    {
        private readonly IMessageHandler messageHanlder;
        private readonly IMessageProccessor messageProccessor;
        private readonly TwilioService twilioService;
        private readonly ILogger logger;

        public TwilioServiceController(IMessageHandler messageHanlder, TwilioService twilioService, ILogger logger , IMessageProccessor messageProccessor)
        {
            this.messageHanlder = messageHanlder;
            this.twilioService = twilioService;
            this.logger = logger;
            this.messageProccessor = messageProccessor;
        }

        public async Task<TwiMLResult> IndexAsync(SmsRequest incomingMessage)
        {
            try
            { 
                await logger.LogWebItem(new WebAPILog()
                {
                    Body = incomingMessage.GetType().Name + System.Text.Json.JsonSerializer.Serialize(incomingMessage)
                    , Module = "TwilioAPIController"
                    , RequestTime = DateTime.Now
                    , Method = "POST"
                });
                
                var receivedMessage = twilioService.GetMessage(incomingMessage);
                var whatsappmessage = await messageHanlder.HanldeMessage(receivedMessage);
                var reply = await messageProccessor.ProcessMessage(whatsappmessage);
                var messagingResponse = new MessagingResponse();
                messagingResponse.Message(reply.Text); 
                return TwiML(messagingResponse);
            }
            catch(Exception ex )
            {
                await logger.LogWebItem(new WebAPILog()
                {
                    Body = $"{ex.Message} Data: {System.Text.Json.JsonSerializer.Serialize(incomingMessage)}"
                     , Module = "TwilioAPIController"
                     , RequestTime = DateTime.Now
                     , Method = "POST"
                });
                var messagingResponse = new MessagingResponse();
                messagingResponse.Message("Message has been received for processing.");
                return TwiML(messagingResponse);
            }

            
        }

        
    }
}
