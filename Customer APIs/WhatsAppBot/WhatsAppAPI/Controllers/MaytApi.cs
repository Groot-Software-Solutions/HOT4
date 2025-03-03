using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.DataModels;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Handlers;
using Infrastructure.Services;
using Infrastructure.Services.MaytApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/MaytApi")]
    [ApiController]
    public class MaytApi : ControllerBase
    {
        private readonly IMessageHandler messageHanlder;
        private readonly IMessageProccessor messageProccessor;
        private readonly ILogger logger;
        private readonly IWhatsAppService service;

        public MaytApi(IMessageHandler messageHanlder, ILogger logger, MaytApiService service, IMessageProccessor messageProccessor)
        {
            this.messageHanlder = messageHanlder;
            this.logger = logger;
            this.service = service;
            this.messageProccessor = messageProccessor;
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook([FromBody] ReceivedMessageModel message)
        {
            try
            {
                if (message.message.fromMe == false)
                {
                    var receivedMessage = service.GetMessage(message);
                    await HandleMessage(receivedMessage);
                }


            }
            catch (Exception ex)
            {
                await logger.LogWebItem(new WebAPILog()
                {
                    Body = $"{ex.Message} Data: {JsonSerializer.Serialize(message)}"
                      ,
                    Module = "MaytAPIController"
                      ,
                    RequestTime = DateTime.Now
                      ,
                    Method = "POST"
                });
                return BadRequest();
            }


            return Ok("");
        }

        //[HttpPost("webhook")]
        //public async Task<IActionResult> WebHook([FromBody] ReceivedMessage message)
        //{
        //    try
        //    {
        //        var receivedMessage = service.GetMessage(message);
        //        await HandleMessage(receivedMessage);

        //    }
        //    catch (Exception ex)
        //    {
        //        await logger.LogWebItem(new WebAPILog()
        //        {
        //            Body = $"{ex.Message} Data: {JsonSerializer.Serialize(message)}"
        //              ,  Module = "MaytAPIController"
        //              ,  RequestTime = DateTime.Now
        //              ,  Method = "POST"
        //        });
        //        return BadRequest();
        //    }


        //    return Ok("");
        //}

        private async Task HandleMessage(Domain.Entities.Message receivedMessage)
        {
            var whatsappmessage = await messageHanlder.HanldeMessage(receivedMessage);
            var reply = await messageProccessor.ProcessMessage(whatsappmessage);
            if (reply.Text.Contains("Unknown") == false)
            {
                _ = await service.ReplyAsync(receivedMessage, reply);
            }
        }
    }
}
