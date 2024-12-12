namespace Hot4.Core.DataViewModels
{
    public class ProfileDiscountModel
    {
        public int ID { get; set; }
        public string Profile { get; set; }
        public string Supplier { get; set; }
        public string ProductCategory { get; set; }
        public string StockAccount { get; set; }
        public decimal Discount { get; set; }
    }
}
