using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.MaytApi.Models
{
    public class SendMessageModel
    { 
        public string to_number { get; set; }
        public string type { get; set; }
        public string message { get; set; } 
    }
}
