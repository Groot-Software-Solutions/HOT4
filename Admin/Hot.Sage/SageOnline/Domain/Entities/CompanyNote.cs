using System;

namespace Sage.Domain.Entities
{
    public class CompanyNote
    {
        public int ID { get; set; }
        public string Subject { get; set; } = "";
        public DateTime EntryDate { get; set; }
        public DateTime ActionDate { get; set; }
        public bool Status { get; set; } = false;
        public string Note { get; set; } = "";
        public bool HasAttachments { get; set; } = false;
    }




}
