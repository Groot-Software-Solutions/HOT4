using System;

namespace Sage.Domain.Entities
{
    public class BankFeedAccount
    {
        public int ID { get; set; } = 0;
        public int BankFeedAccountGroupId { get; set; } = 0;
        public BankFeedAccountGroup BankFeedAccountGroup { get; set; } = new BankFeedAccountGroup();
        public string Description { get; set; } = "";
        public string Identifier { get; set; } = "";
        public DateTime LastRefreshDate { get; set; }
        public DateTime FirstImportDate { get; set; }
        public int BankAccountId { get; set; } = 0;
        public string BankAccountName { get; set; } = "";
        public int LastRefreshStatusId { get; set; } = 0;
    }




}
