namespace Sage.Domain.Entities
{
    public class Supplier
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public SupplierCategory Category { get; set; }
        public string TaxReference { get; set; } = "";
        public string ContactName { get; set; } = "";
        public string Telephone { get; set; } = "";
        public string Fax { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Email { get; set; } = "";
        public string WebAddress { get; set; } = "";
        public bool Active { get; set; } = true;
        public decimal Balance { get; set; } = 0;
        public decimal CreditLimit { get; set; } = 0;
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
    }


}
