using System;

namespace Sage.Domain.Entities
{ 
        public class CustomerSaveRequest
    {
            public string Name { get; set; }
            public CustomerCategory Category { get; set; }
            // Public Property SalesRepresentativeId As Integer = 0
            // Public Property SalesRepresentative As New SalesRepresentative
            public string TaxReference { get; set; }
            public string ContactName { get; set; }
            public string Telephone { get; set; }
            public string Fax { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string WebAddress { get; set; }
            public bool Active { get; set; }
            public decimal Balance { get; set; }
            public decimal CreditLimit { get; set; }
            public int CommunicationMethod { get; set; }
            public string PostalAddress01 { get; set; }
            public string PostalAddress02 { get; set; }
            public string PostalAddress03 { get; set; }
            public string PostalAddress04 { get; set; }
            public string PostalAddress05 { get; set; }
            public string DeliveryAddress01 { get; set; }
            public string DeliveryAddress02 { get; set; }
            public string DeliveryAddress03 { get; set; }
            public string DeliveryAddress04 { get; set; }
            public string DeliveryAddress05 { get; set; }
            public bool AutoAllocateToOldestInvoice { get; set; }
            public bool EnableCustomerZone { get; set; }
            public string CustomerZoneGuid { get; set; }
            public bool CashSale { get; set; }
            public string TextField1 { get; set; }
            public string TextField2 { get; set; }
            public string TextField3 { get; set; }
            public decimal NumericField1 { get; set; }
            // Public Property NumericField2 As Decimal = 0
            // Public Property NumericField3 As Decimal = 0
            public bool YesNoField1 { get; set; }
            public bool YesNoField2 { get; set; }
            public bool YesNoField3 { get; set; }
            // Public Property DateField1 As Date
            // Public Property DateField2 As Date
            // Public Property DateField3 As Date
            // Public Property DefaultPriceListId As Integer = 0
            // Public Property DefaultPriceList As New AdditionalPriceList
            public string DefaultPriceListName { get; set; }
            public bool AcceptsElectronicInvoices { get; set; }
            public DateTime Modified { get; set; }
            public DateTime Created { get; set; }
            // Public Property BusinessRegistrationNumber As String = ""
            // Public Property TaxStatusVerified As Date
            // Public Property CurrencyId As Integer = 0
            public bool HasActivity { get; set; }
            public decimal DefaultDiscountPercentage { get; set; }
            // Public Property DefaultTaxTypeId As Integer = 0
            // Public Property DefaultTaxType As New TaxType
            // Public Property DueDateMethodId As Integer = 0
            // Public Property DueDateMethodValue As Integer = 0
            // Public Property CurrencySymbol As String = ""
            public int ID { get; set; }

            public CustomerSaveRequest(Customer x)
            {
                Name = x.Name;
                Category = x.Category;
                TaxReference = x.TaxReference;
                ContactName = x.ContactName;
                Telephone = x.Telephone;
                Fax = x.Fax;
                Mobile = x.Mobile;
                Email = x.Email;
                WebAddress = x.WebAddress;
                Active = x.Active;
                Balance = x.Balance;
                CreditLimit = x.CreditLimit;
                CommunicationMethod = x.CommunicationMethod;
                PostalAddress01 = x.PostalAddress01;
                PostalAddress02 = x.PostalAddress02;
                PostalAddress03 = x.PostalAddress03;
                PostalAddress04 = x.PostalAddress04;
                PostalAddress05 = x.PostalAddress05;
                DeliveryAddress01 = x.DeliveryAddress01;
                DeliveryAddress02 = x.DeliveryAddress02;
                DeliveryAddress03 = x.DeliveryAddress03;
                DeliveryAddress04 = x.DeliveryAddress04;
                DeliveryAddress05 = x.DeliveryAddress05;
                AutoAllocateToOldestInvoice = x.AutoAllocateToOldestInvoice;
                EnableCustomerZone = x.EnableCustomerZone;
                CustomerZoneGuid = x.CustomerZoneGuid;
                CashSale = x.CashSale;
                TextField1 = x.TextField1;
                TextField2 = x.TextField2;
                TextField3 = x.TextField3;
                NumericField1 = x.NumericField1;
                YesNoField1 = x.YesNoField1;
                YesNoField2 = x.YesNoField2;
                YesNoField3 = x.YesNoField3;
                DefaultPriceListName = x.DefaultPriceListName;
                AcceptsElectronicInvoices = x.AcceptsElectronicInvoices;
                Modified = x.Modified;
                Created = x.Created;
                HasActivity = x.HasActivity;
                DefaultDiscountPercentage = x.DefaultDiscountPercentage;
                ID = x.ID;
            }
        }
      
}
