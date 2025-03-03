namespace Hot.API.Client.Models
{
    public class ZesaToken
    {
        public string Token { get; set; }
        public decimal Units { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Levy { get; set; }
        public decimal Arrears { get; set; }
        public decimal TaxAmount { get; set; }
        public string ZesaReference { get; set; }

    }
}
