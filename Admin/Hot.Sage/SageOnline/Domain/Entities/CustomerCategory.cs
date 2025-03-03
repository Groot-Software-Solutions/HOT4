using System;

namespace Sage.Domain.Entities
{
    public class CustomerCategory
    {
        public string Description { get; set; } = "";
        public long ID { get; set; } = 0;
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; } 
    }



}
