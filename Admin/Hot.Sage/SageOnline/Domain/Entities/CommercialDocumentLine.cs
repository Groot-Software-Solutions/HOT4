using Sage.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Domain.Entities
{
    public class CommercialDocumentLine
    {
        public int SelectionId { get; set; } = 0;
        public int TaxTypeId { get; set; } = 0;
        public long ID { get; set; } = 0;
        public string Description { get; set; } = "";
        public CommercialDocumentLineType LineType { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;
        public string Unit { get; set; } = "";
        public decimal UnitPriceExclusive { get; set; } = 0;
        public decimal UnitPriceInclusive { get; set; } = 0;
        public decimal TaxPercentage { get; set; } = 0;
        public decimal DiscountPercentage { get; set; } = 0;
        public decimal Exclusive { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal Tax { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public string Comments { get; set; } = "";
        public int AnalysisCategoryId1 { get; set; } = 0;
        public int AnalysisCategoryId2 { get; set; } = 0;
        public int AnalysisCategoryId3 { get; set; } = 0;
        public string TrackingCode { get; set; } = "";
        public int CurrencyId { get; set; } = 0;
        public decimal UnitCost { get; set; } = 0;
         
    }




}
