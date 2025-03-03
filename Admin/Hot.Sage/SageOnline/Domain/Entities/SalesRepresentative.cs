using System;

namespace Sage.Domain.Entities
{
    public class SalesRepresentative
    {
        public int ID { get; set; } = 0;
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Name { get; set; } = "";
        public bool Active { get; set; } = true;
        public string Email { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Telephone { get; set; } = "";
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }


    }


}
