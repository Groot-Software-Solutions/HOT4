using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DataModels
{
    public class LogItem
    {
        public long Id { get; set; }

        public string Module { get; set; }

        public string Method { get; set; }

        public string Data { get; set; }

        public long? Reference { get; set; }

    }
}
