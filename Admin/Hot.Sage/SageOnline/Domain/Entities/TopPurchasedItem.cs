namespace Sage.Domain.Entities
{
    public class TopPurchasedItem
    {
        public int ItemId { get; set; } = 0;
        public string Description { get; set; } = "";
        public decimal Exclusive { get; set; } = 0;
        public decimal Total { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;
    }


}
