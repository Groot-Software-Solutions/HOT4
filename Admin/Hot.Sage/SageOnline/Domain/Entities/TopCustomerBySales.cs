namespace Sage.Domain.Entities
{
    public class TopCustomerBySales
    {
        public int CustomerId { get; set; } = 0;
        public string Name { get; set; } = "";
        public decimal Exclusive { get; set; } = 0;
        public decimal Total { get; set; } = 0;
    }


}
