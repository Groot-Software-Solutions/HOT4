using Hot.API.Client.Ecocash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;



namespace Hot.API.Client.Models
{
    public class EcocashFundingRequest
    {
        public decimal Amount { get; set; }
        public string TargetMobile { get; set; }
        public string OnBehalfOf { get; set; } = "Comm Shop";
        public EcocashAccounts Account { get; set; } = EcocashAccounts.Default;

    }
}

