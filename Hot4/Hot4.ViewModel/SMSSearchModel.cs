namespace Hot4.ViewModel
{
    public class SMSSearchModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Mobile { get; set; }
        public string MessageText { get; set; }
        public int SmppId { get; set; }
        public byte StateId { get; set; }
    }
}
