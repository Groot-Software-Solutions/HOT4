namespace Sage.Domain.Entities
{
    public class TopSellingByValueOnHand
    {
        public int ItemId { get; set; } = 0;
        public string Description { get; set; } = "";
        public decimal QuantityOnHand { get; set; } = 0;
        public decimal AverageCost { get; set; } = 0;
        public decimal ValueOnHand { get; set; } = 0;
    }


}
