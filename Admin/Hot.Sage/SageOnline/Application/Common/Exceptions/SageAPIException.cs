using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Application.Common.Exceptions
{
    public class SageAPIException : Exception
    {
        public SageAPIException(string name, object data)
            : base($"API Endpoint Exception \"{name}\" {data}")
        {
        }
    }

}
