using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.MaytApi.Models
{
    public class SendMessageResponseModel
    { 
        public bool success { get; set; }
        public SendMessageResponseDataModel data { get; set; }
    }

}
