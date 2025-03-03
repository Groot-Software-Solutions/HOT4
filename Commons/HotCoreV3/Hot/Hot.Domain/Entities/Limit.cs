namespace Hot.Domain.Entities
{
    public class Limit
    {
        public long LimitId { get; set; }
        public byte NetworkId { get; set; }
        public long AccountId { get; set; }
        public int LimitTypeId { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal Monthly { get; set; }
    }
}
