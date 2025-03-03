namespace Hot.Application.Common.Models
{
    public class StatementRequest
    {
        public long AccountID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
