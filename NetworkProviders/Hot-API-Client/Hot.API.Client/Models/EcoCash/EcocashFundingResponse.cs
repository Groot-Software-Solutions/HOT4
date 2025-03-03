using Hot.API.Client.Ecocash.Domain.Entities;
using System;
namespace Hot.API.Client.Models
{
    public class EcocashFundingResponse : Response
    { 
        public Transaction TransactionData { get; set; }
    }
}

