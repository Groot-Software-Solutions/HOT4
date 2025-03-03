using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Application.Common.Exceptions
{
    public class APIException : Exception
    { 
            public APIException(string name, object data)
                : base($"API Endpoint Exception \"{name}\" {data}")
            {
            } 
    }
}
