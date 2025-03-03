using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Domain.Entities
{
    public class ItemNote
    {
        public int ID { get; set; } = 0;
        public int ItemId { get; set; } = 0;
        public string Subject { get; set; } = "";
        public DateTime EntryDate { get; set; }
        public DateTime ActionDate { get; set; }
        public bool Status { get; set; } = true;
        public string Note { get; set; } = "";
        public bool HasAttachments { get; set; } = false;


    }


}
