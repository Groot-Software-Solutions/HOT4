using Sage.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Domain.Entities
{
    public class CustomerReceipt
    {
        public long Id { get; set; } = 0;
        public long CustomerId { get; set; } = 0;
        public DateTime Date { get; set; }
        public string Payee { get; set; } = "";
        public string DocumentNumber { get; set; } = "";
        public string Reference { get; set; } = "";
        public string Description { get; set; } = "";
        public string Comments { get; set; } = "";
        public decimal Total { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal TotalUnallocated { get; set; } = 0;
        public bool Reconciled { get; set; } = false;
        public int BankAccountId { get; set; } = 0;
        public PaymentMethod PaymentMethod { get; set; } = new PaymentMethod();
        public int TaxPeriodId { get; set; } = 0;
        public bool Editable { get; set; } = true;
        public bool Accepted { get; set; } = true;
        public bool Locked { get; set; } = false;
        public int AnalysisCategoryId1 { get; set; } = 0;
        public int AnalysisCategoryId2 { get; set; } = 0;
        public int AnalysisCategoryId3 { get; set; } = 0;
        public bool Printed { get; set; } = false;
        public string BankUniqueIdentifier { get; set; } = "";
        public int ImportTypeId { get; set; } = 0;
        public int BankImportMappingId { get; set; } = 0;
        public int BankAccountCurrencyId { get; set; } = 0;
        public decimal BankAccountExchangeRate { get; set; } = 0;
        public int CustomerCurrencyId { get; set; } = 0;
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public Customer Customer { get; set; } = new Customer();
        public SalesRepresentative SalesRepresentative { get; set; } = new SalesRepresentative();
        public BankAccount BankAccount { get; set; } = new BankAccount();


    }


}
