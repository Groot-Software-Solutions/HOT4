using Infrastructure.Services.MaytApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class MessageNotificationModel
    { 
        public ReceivedMessage body { get; set; }
        public string webhook { get; set; }
    }

}
