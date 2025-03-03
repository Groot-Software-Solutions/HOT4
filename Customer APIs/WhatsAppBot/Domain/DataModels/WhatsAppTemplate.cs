using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataModels
{
    public class WhatsAppTemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public byte[] ImageData { get; set; }

    }
}
