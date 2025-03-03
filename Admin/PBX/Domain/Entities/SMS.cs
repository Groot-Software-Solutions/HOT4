using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Entities
{
    public class SMS
    {
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; } 
        public SMSDirection DirectionId { get; set; }
        public SMSStatus StatusId { get; set; }
        public string? User { get; set; }

    }
}
