using System;

namespace Sage.Domain.Entities
{
    public class Allocation
    {
        public long ID { get; set; } = 0;
        public int DocumentHeaderId_Source { get; set; } = 0;
        public int DocumentHeaderId_Allocation { get; set; } = 0;
        public long SourceDocumentId { get; set; } = 0;
        public long AllocatedToDocumentId { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public int Type { get; set; } = 0;
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

    }


}
