using Sage.Domain.Enums;
using System;

namespace Sage.Domain.Entities
{
    public class AgeingTransaction
    {
        public int DocumentHeaderId { get; set; } = 0;
        public int DocumentTypeId { get; set; } = 0;
        public string DocumentNumber { get; set; } = "";
        public DocumentType DocumentType { get; set; }
        public string Comment { get; set; } = "";
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
    }



}
