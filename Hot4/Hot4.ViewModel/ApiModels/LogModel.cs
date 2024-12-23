namespace Hot4.ViewModel.ApiModels
{
    public class LogModel
    {
        public long LogId { get; set; }

        public required DateTime LogDate { get; set; }

        public required string LogModule { get; set; }

        public required string LogObject { get; set; }

        public required string LogMethod { get; set; }

        public required string LogDescription { get; set; }

        public string? Idtype { get; set; }

        public long? Idnumber { get; set; }
    }
}
