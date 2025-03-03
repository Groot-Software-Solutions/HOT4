using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Common
{
    public class APIException : Exception
    {
        public APIException(string name, object data)
            : base($"API Endpoint Exception \"{name}\" {data}")
        {
        }
    }
}
