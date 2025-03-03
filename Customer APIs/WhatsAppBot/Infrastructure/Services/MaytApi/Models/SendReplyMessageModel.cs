using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.MaytApi.Models
{
    public class SendReplyMessageModel : SendMessageModel
    {
        public string reply_to { get; set; }
    }
}
