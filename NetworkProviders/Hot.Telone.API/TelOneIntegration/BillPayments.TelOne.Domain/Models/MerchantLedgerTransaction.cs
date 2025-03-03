using System;

namespace TelOne.Domain.Models
{
    public class MerchantLedgerTransaction
    {
        public int ID { get; set; }
        public string RefNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string VoucherNum { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string Narrative { get; set; }
        public bool Cr { get; set; }
        public int TransType { get; set; }
        public int Service { get; set; }
        public string Currency { get; set; }
    }
}
