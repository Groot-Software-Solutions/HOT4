namespace Hot4.ViewModel
{
    public class RechargeAggSearchModel
    {
        public long AccountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
