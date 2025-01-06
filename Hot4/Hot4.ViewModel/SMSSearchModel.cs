namespace Hot4.ViewModel
{
    public class SMSSearchModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Mobile { get; set; }
        public string MessageText { get; set; }
        public int SmppId { get; set; }
        public int StateId { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
