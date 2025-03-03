using BillPayments.Domain.Enums;
using System.Reflection.Metadata.Ecma335;

namespace BillPayments.Domain.Models.PurchaseToken
{
    public class ResendPurchaseTokenRequest : BaseRequest
    {
        public int TransactionAmount { get; set; }
        public string MerchantName { get; set; }
        public string UtilityAccount { get; set; }
        public string ProductName { get; set; }
        public string CurrencyCode { get; set; }
        public string OriginalReference { get; set; }
        public new string ProcessingCode { get; set; } = ProcessingCodes.TokenPurchaseRequest;
        public new string Mti { get; set; } = MessageTypeIndicator.TransactionResendRequest;
    }
}