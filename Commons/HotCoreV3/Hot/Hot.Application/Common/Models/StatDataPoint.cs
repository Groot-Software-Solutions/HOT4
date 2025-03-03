namespace Hot.Application.Common.Models
{
    public class StatDataPoint
    {
        public StatDataPoint()
        {
        }

        public StatDataPoint(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public long x { get; set; }
        public long y { get; set; }
    }
}
