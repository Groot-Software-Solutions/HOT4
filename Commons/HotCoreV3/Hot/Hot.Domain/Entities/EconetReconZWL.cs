using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Domain.Entities
{
    public class EconetReconZWL
    {
        public string Reference { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
        public string RechargeDateStr { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ReturnCode { get; set; } = string.Empty;
    }
}
