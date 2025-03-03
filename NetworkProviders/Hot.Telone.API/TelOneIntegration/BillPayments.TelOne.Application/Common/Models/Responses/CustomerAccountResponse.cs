namespace TelOne.Application.Common.Models
{
    public class CustomerAccountResponse : Response
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountAddress { get; set; } 
    }
}
