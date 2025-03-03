using Sage.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sage.Domain.Entities
{
    public class AccountBalance
    {
        public int ID { get; set; } = 0;
        public AccountType Type { get; set; } = new AccountType();
        public DateTime Date { get; set; }
        public string Description { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public string CategoryDescription { get; set; } = "";
        public int AnalysisCategoryId { get; set; } = 0;
        public string AnalysisCategoryDescription { get; set; } = "";
        public decimal Debit { get; set; } = 0;
        public decimal Credit { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public List<BudgetItemPeriod> BudgetItemPeriods { get; set; } = new List<BudgetItemPeriod>();
    }



}
