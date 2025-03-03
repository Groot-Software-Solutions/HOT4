
using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Domain.Entities
{
    

    public class AccountReceipt
    {
        public int ID { get; set; } = 0;
        public int AccountId { get; set; } = 0;
        public DateTime Date { get; set; }
        public string Payee { get; set; } = "";
        public string Description { get; set; } = "";
        public string Reference { get; set; } = "";
        public int TaxTypeId { get; set; } = 0;
        public string Comments { get; set; } = "";
        public decimal Exclusive { get; set; } = 0;
        public decimal Tax { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public bool Reconciled { get; set; } = false;
        public int BankAccountId { get; set; } = 0;
        public int AnalysisCategoryId1 { get; set; } = 0;
        public int AnalysisCategoryId2 { get; set; } = 0;
        public int AnalysisCategoryId3 { get; set; } = 0;
        public int ParentId { get; set; } = 0;
        public bool Accepted { get; set; } = true;
        public string BankUniqueIdentifier { get; set; } = "";
        public int ImportTypeId { get; set; } = 0;
        public int BankImportMappingId { get; set; } = 0;
        public int BankAccountCurrencyId { get; set; } = 0;
        public decimal BankAccountExchangeRate { get; set; } = 0;
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }

    }


}
