using System;

namespace Sage.Domain.Entities
{
    public class CustomerNote
    {
        public int ID { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public string Subject { get; set; } = "";
        public DateTime EntryDate { get; set; }
        public DateTime ActionDate { get; set; }
        public bool Status { get; set; } = false;
        public string Note { get; set; } = "";
        public bool HasAttachments { get; set; } = false; 
    }



}
