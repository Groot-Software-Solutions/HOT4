using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public MessageType Type { get; set; }
        public string PhoneNumber { get; set; }
        public string ConversationId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }


    }
}
