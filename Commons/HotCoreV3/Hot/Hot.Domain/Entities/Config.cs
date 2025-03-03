using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Config
    {
        public byte ConfigId { get; set; }
        public int ProfileId_NewSmsDealer { get; set; }
        public int ProfileId_NewWebDealer { get; set; }
        public decimal MinRecharge { get; set; }
        public decimal MaxRecharge { get; set; }
        public bool PrepaidEnabled { get; set; }
        public decimal MinTransfer { get; set; }
        public int DuplicateSmsMinutes { get; set; }
        public string LicenseKey { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
    }
}
