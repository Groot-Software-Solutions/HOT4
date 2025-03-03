using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelOne.Application.Common.Exceptions
{
    public class APIException : Exception
    {
        public APIException(string name, object data)
            : base($"API Endpoint Exception \"{name}\" {data}")
        {
        }
    }
}
