using System;

namespace Hot.Lib.Entities
{
    public class Log
    {
        public long LogId { get; set; }
        public DateTime LogDate { get; set; }
        public string LogModule { get; set; } = string.Empty;
        public string LogObject { get; set; } = string.Empty;
        public string LogMethod { get; set; } = string.Empty;
        public string LogDescription { get; set; } = string.Empty;
        public string IdType { get; set; } = string.Empty;
        public long? IdNumber { get; set; }
    }
}
