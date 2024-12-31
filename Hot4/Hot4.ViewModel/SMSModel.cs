namespace Hot4.ViewModel
{
    public class SMSModel
    {
        public long SMSId { get; set; }
        public byte? SmppId { get; set; }
        public byte StateId { get; set; }
        public string State { get; set; }
        public byte PriorityId { get; set; }
        public string Priority { get; set; }
        public bool Direction { get; set; }
        public string Mobile { get; set; }
        public string SMSText { get; set; }
        public DateTime SMSDate { get; set; }
        public DateTime? InsertDate { get; set; }
        public long? SMSIDIn { get; set; }
    }
}
