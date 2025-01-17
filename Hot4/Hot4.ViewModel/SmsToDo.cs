namespace Hot4.ViewModel
{
    public class SmsToDo
    {
        public long Smsid { get; set; }
        public byte? SmppId { get; set; }
        public byte StateId { get; set; }
        public byte PriorityId { get; set; }
        public bool Direction { get; set; }
        public required string Mobile { get; set; }
        public required string Smstext { get; set; }
        public DateTime Smsdate { get; set; }
        public long? SmsidIn { get; set; }
        public DateTime? InsertDate { get; set; }
    }
}
