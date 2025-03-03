using Domain.Entities;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataModels
{
    public class WhatsAppMessage
    {
        public long Id { get; set; }

        public string Mobile { get; set; }

        public string Message { get; set; }

        public string ConversationId { get; set; }

        public RequestType TypeId { get; set; }

        public DateTime MessageDate { get; set; }

        public State StateId { get; set; }

        public Message ToMessage()
        {
            return new Message()
            {
                Id = Id.ToString()
                , ConversationId = ConversationId
                , Text = Message
                , Date = MessageDate
                , PhoneNumber = Mobile
                , Type = MessageType.Text
            };

        }
    }
}
