using System;

namespace Sage.Domain.Entities
{
    public partial class Customer
    {
        public string Name { get; set; } = "";
        public CustomerCategory Category { get; set; } = new CustomerCategory();
        public int SalesRepresentativeId { get; set; } = 0;
        public SalesRepresentative SalesRepresentative { get; set; } = new SalesRepresentative();
        public string TaxReference { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string Telephone { get; set; } = "";
        public string Fax { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Email { get; set; } = "";
        public string WebAddress { get; set; } = "";
        public bool Active { get; set; } = true;
        public decimal Balance { get; set; }
        public decimal CreditLimit { get; set; } = 0;
        public int CommunicationMethod { get; set; } = 0;
        public string PostalAddress01 { get; set; } = "";
        public string PostalAddress02 { get; set; } = "";
        public string PostalAddress03 { get; set; } = "";
        public string PostalAddress04 { get; set; } = "";
        public string PostalAddress05 { get; set; } = "";
        public string DeliveryAddress01 { get; set; } = "";
        public string DeliveryAddress02 { get; set; } = "";
        public string DeliveryAddress03 { get; set; } = "";
        public string DeliveryAddress04 { get; set; } = "";
        public string DeliveryAddress05 { get; set; } = "";
        public bool AutoAllocateToOldestInvoice { get; set; } = true;
        public bool EnableCustomerZone { get; set; } = true;
        public string CustomerZoneGuid { get; set; } = "1400147f-b920-4d65-85c8-acf7c2708781";
        public bool CashSale { get; set; } = false;
        public string TextField1 { get; set; } = "";
        public string TextField2 { get; set; } = "";
        public string TextField3 { get; set; } = "";
        public decimal NumericField1 { get; set; } = 0;
        public decimal NumericField2 { get; set; } = 0;
        public decimal NumericField3 { get; set; } = 0;
        public bool YesNoField1 { get; set; } = false;
        public bool YesNoField2 { get; set; } = false;
        public bool YesNoField3 { get; set; } = false;
        public DateTime DateField1 { get; set; }
        public DateTime DateField2 { get; set; }
        public DateTime DateField3 { get; set; }
        public int DefaultPriceListId { get; set; } = 0;
        public AdditionalPriceList DefaultPriceList { get; set; } = new AdditionalPriceList();
        public string DefaultPriceListName { get; set; } = "";
        public bool AcceptsElectronicInvoices { get; set; } = true;
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public string BusinessRegistrationNumber { get; set; } = "";
        public DateTime TaxStatusVerified { get; set; }
        public int CurrencyId { get; set; } = 0;
        public bool HasActivity { get; set; } = false;
        public decimal DefaultDiscountPercentage { get; set; } = 0;
        public int DefaultTaxTypeId { get; set; } = 0;
        public TaxType DefaultTaxType { get; set; } = new TaxType();
        public int DueDateMethodId { get; set; } = 0;
        public int DueDateMethodValue { get; set; } = 0;
        public string CurrencySymbol { get; set; } = "";
        public int ID { get; set; } = 0;



    }


}
