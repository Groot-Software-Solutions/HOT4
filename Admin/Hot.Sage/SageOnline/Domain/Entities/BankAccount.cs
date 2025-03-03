using System;

namespace Sage.Domain.Entities
{
    public class BankAccount
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string BankName { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public string BranchNumber { get; set; } = "";
        public BankAccountCategory Category { get; set; } = new BankAccountCategory();
        public bool Active { get; set; } = true;
        public bool Default { get; set; } = false;
        public decimal Balance { get; set; } = 0;
        public string Description { get; set; } = "";
        public BankFeedAccount BankFeedAccount { get; set; } = new BankFeedAccount();
        public DateTime LastTransactionDate { get; set; }
        public DateTime LastImportDate { get; set; }
        public bool HasTransactionsWaitingForReview { get; set; } = false;

    }
      
}
