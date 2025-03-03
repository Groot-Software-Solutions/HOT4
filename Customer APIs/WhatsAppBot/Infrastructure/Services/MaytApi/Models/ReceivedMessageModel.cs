using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.MaytApi.Models
{

    public class ReceivedMessageModel
    {
        public string product_id { get; set; }
        public int phone_id { get; set; }
        public Message message { get; set; }
        public User user { get; set; }
        public string conversation { get; set; }
        public string receiver { get; set; }
        public int timestamp { get; set; }
        public string type { get; set; }
        public string reply { get; set; }
        public Quoted quoted { get; set; }
    }

    

}
