using System;
using System.Collections.Generic;

namespace Sage.Domain.Entities
{
    public class TaxInvoice
    {
        public DateTime DueDate { get; set; }
        public string FromDocumenta { get; set; } = "";
        public int FromDocumentId { get; set; } = 0;
        public int FromDocumentTypeId { get; set; } = 0;
        public bool AllowOnlinePayment { get; set; } = false;
        public long Id { get; set; } = 0;
        public bool Paid { get; set; } = false;
        public string Status { get; set; } = "";
        public bool Locked { get; set; } = false;
        public long CustomerID { get; set; } = 0;
        public string CustomerName { get; set; } = "";
        public Customer Customer { get; set; } = new Customer();
        public int SalesRepresentativeId { get; set; } = 0;
        public SalesRepresentative SalesRepresentative { get; set; } = new SalesRepresentative();
        public int StatusId { get; set; } = 0;
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public DateTime Date { get; set; }
        public bool Inclusive { get; set; } = false;
        public decimal DiscountPercentage { get; set; } = 0;
        public string TaxReference { get; set; } = "";
        public string DocumentNumber { get; set; } = "";
        public string Message { get; set; } = "";
        public decimal Discount { get; set; } = 0;
        public decimal Exclusive { get; set; } = 0;
        public decimal Tax { get; set; } = 0;
        public decimal Rounding { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal AmountDue { get; set; } = 0;
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
        public int CurrencyId { get; set; } = 0;
        public decimal ExchangeRate { get; set; } = 0;
        public int TaxPeriodId { get; set; } = 0;
        public bool Editable { get; set; } = false;
        public bool HasAttachments { get; set; } = false;
        public bool HasNotes { get; set; } = false;
        public bool HasAnticipatedDate { get; set; } = false;
        public DateTime AnticipatedDate { get; set; }
        public List<CommercialDocumentLine> Lines { get; set; } = new List<CommercialDocumentLine>();

    }


}
