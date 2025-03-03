using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Ecocash.Application.Common.Models
{
    public class FundWalletRequest
    {
        public string TargetMobile { get; set; }
        public string AccessCode { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceId { get; set; }
        public string Remarks { get; set; }
        public string OnBehalfOf { get; set; }
        public byte EcocashAccount { get; set; }
    }
}
