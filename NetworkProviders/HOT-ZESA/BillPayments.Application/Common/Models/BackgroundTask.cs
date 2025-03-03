using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Application.Models
{
    public class BackgroundTask
    {
        public int Id { get; set; }
        public string EntityBody { get; set; }
        public string EntityType { get; set; }
        public string LastResponse { get; set; }
        public DateTime DateCreated { get; set; }
        public int NumberOfRetries { get; set; }
        public bool RetrySucceeded { get; set; }
        public DateTime? DateOfLastRetry { get; set; }
    }
}
