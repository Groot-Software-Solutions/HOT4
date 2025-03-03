
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{ 
    public class MailServiceOptions 
    {
        public bool AllowedToSend { get; set; }

        public string SMTPHost { get; set; } = string.Empty;
        public int SMTPPort { get; set; }
        public bool SMTPTLSEnabled { get; set; }
        public bool SMTPStartTLSEnabled { get; set; }
        public bool SMTPAuthRequired { get; set; }

        public string IMAPHost { get; set; } = string.Empty;
        public int IMAPPort { get; set; } 
        public bool IMAPTLSEnabled { get; set; }

        public string POP3Host { get; set; } = string.Empty;
        public int POP3Port { get; set; }
        public bool POP3TLSEnabled { get; set; }

        public string AppClientId { get; set; } = string.Empty;
        public string ScopeUrl { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public static class MailServiceOptionsExtensions
    {
        public static IServiceCollection AddMailSereviceOptions(this IServiceCollection collection,
               Action<MailServiceOptions> config)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (config == null) throw new ArgumentNullException(nameof(config));

            collection.Configure(config);
            return collection;
        }
    }
}
