using Hot.Nyaradzo.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hot.Nyaradzo.Domain.Entities
{
    public class ReversalResult : TransactionResult
    {
        public string Code { get; set; } = string.Empty;
    }
}
