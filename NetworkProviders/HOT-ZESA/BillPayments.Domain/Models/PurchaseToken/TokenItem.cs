namespace BillPayments.Domain.Model.PurchaseToken
{
    public class TokenItem
        {
            public string ZESAReference { get; set; }
            public string Token { get; set; }
            public decimal Units { get; set; }
            public decimal NetAmount { get; set; }
            public decimal Arrears { get; set; }
            public decimal Levy { get; set; }
            public decimal TaxAmount { get; set; }
        }
    
}
