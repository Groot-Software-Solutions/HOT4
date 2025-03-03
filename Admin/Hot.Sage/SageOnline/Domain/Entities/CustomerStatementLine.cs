using System;

namespace Sage.Domain.Entities
{
    public class CustomerStatementLine
    {
        public int DocumentHeaderId { get; set; } = 0;
        public int DocumentTypeId { get; set; } = 0;
        public DateTime Date { get; set; }
        public DateTime DateAllocation { get; set; }
        public string DocumentNumber { get; set; } = "";
        public string DocumentNumberAllocation { get; set; } = "";
        public string Reference { get; set; } = "";
        public string Comment { get; set; } = "";
    }


}
