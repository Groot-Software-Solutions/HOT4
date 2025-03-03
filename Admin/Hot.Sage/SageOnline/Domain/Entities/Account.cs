using System;

namespace Sage.Domain.Entities
{
    public class Account
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public AccountCategory Category { get; set; } = new AccountCategory();
        public bool Active { get; set; } = true;
        public decimal Balance { get; set; } = 0;
        public string Description { get; set; } = "";
        public bool UnallocatedAccount { get; set; } = false;
        public bool IsTaxLocked { get; set; } = false;
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public int AccountType { get; set; } = 0;
        public bool HasActivity { get; set; } = false;
        public int DefaultTaxTypeId { get; set; } = 0;
        public TaxType DefaultTaxType { get; set; } = new TaxType();
    }

    


}
