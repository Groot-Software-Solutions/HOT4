using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
   
    [Route("api/WhatsAppLink")]
    [ApiController]
    public class WhatsAppLink : Controller
    {
        private readonly IMessageHandler messageHanlder;
        private readonly IMessageProccessor messageProccessor;
        private readonly ILogger logger;
        private readonly IWhatsAppService service;
        public IActionResult Index()
        {
            return View();
        }

        public Response SendMessage(Message message)
        { 

            try
            {
                return new Response
                {
                    Status = Domain.Enum.State.Failed
                , Message = "not implemented"
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                   Status = Domain.Enum.State.Failed
                 , Message = ex.Message
                };
            }
          
        }
    }

}
