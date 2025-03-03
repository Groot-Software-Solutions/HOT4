using Sage.Application.Common.Helpers;
using Sage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Application.Common.Models
{
    public class HotSageAccount
    {
        public long AccountID { get; set; }
        public int ProfileID { get; set; }
        public string AccountName { get; set; }
        public string NationalID { get; set; }
        public string Email { get; set; }
        public string ReferredBy { get; set; }
        public DateTime InsertDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string VatNumber { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public long? SageID { get; set; }
        public byte? InvoiceFreq { get; set; }
        public int? IsUSDAccount { get; set; }
        public Customer Customer
        {
            get
            {
                return new Customer()
                {
                    Name = AccountName,
                    SalesRepresentativeId = SageSystemOptions.HotSalesRepId,
                    NumericField1 = AccountID,
                    TextField1 = NationalID,
                    Email = Email,
                    DeliveryAddress01 = Address1,
                    DeliveryAddress02 = Address2,
                    DeliveryAddress03 = City,
                    PostalAddress01 = Address1,
                    PostalAddress02 = Address2,
                    PostalAddress03 = City,
                    ContactName = ContactName,
                    Telephone = ContactName,
                    TaxReference = VatNumber,
                    Active = true,
                    DefaultTaxTypeId = 1372026,
                    CommunicationMethod = 2,
                    Category = new CustomerCategory() {
                        ID = HotSageHelper.GetCategoryFromProfileID(ProfileID), 
                        Description = ProfileID.ToString() 
                    },
                    CurrencyId = (IsUSDAccount ?? 0) == 1 ? 151 : 169
                };
            }
        }

    }
}
