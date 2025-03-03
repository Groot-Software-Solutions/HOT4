using System;

namespace Sage.Domain.Entities
{
    public class SupplierNote
    {
        public int ID { get; set; } = 0;
        public int SupplierId { get; set; } = 0;
        public string Subject { get; set; } = "";
        public DateTime EntryDate { get; set; }
        public DateTime ActionDate { get; set; }
        public bool Status { get; set; } = true;
        public string Note { get; set; } = "";
        public bool HasAttachments { get; set; } = false;
    }


}
