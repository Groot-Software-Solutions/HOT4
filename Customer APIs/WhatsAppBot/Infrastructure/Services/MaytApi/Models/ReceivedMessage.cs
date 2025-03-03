
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.MaytApi.Models

{
    //public class ReceivedMessage
    //{
    //    public string product_id { get; set; }
    //    public int phone_id { get; set; }
    //    public Message message { get; set; }
    //    public User user { get; set; }
    //    public string conversation { get; set; }
    //    public string receiver { get; set; }
    //    public int timestamp { get; set; }
    //    public string type { get; set; }
    //    public string reply { get; set; }
    //    public Quoted quoted { get; set; }
    //    public int phoneId { get; set; }
    //    public string productId { get; set; }
    //    public string conversation_name { get; set; }
    //}


    public class ReceivedMessage
    {
        public MessageBody body { get; set; }
        public string webhook { get; set; }
        public MessageResponse response { get; set; }
    }


}
