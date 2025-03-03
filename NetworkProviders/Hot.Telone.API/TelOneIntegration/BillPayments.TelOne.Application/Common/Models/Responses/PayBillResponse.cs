using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Application.Common.Models
{
    public class PayBillResponse 
    { 
        public string return_description { get; set; }
        public string return_result { get; set; }
        public string return_message { get; set; }
        public int return_code { get; set; }
        public object Balance { get; set; }
    }
}
