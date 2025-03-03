using Hot.Nyaradzo.Application.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;

namespace Hot.Nyaradzo.Application.Common.Models
{
    public class ServiceOptions
    {
        public string BankCode { get; set; } = string.Empty;
        public string AuthKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;

    }
}
