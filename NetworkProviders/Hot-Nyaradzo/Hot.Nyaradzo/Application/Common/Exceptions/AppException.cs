using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Nyaradzo.Application.Common.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string name, object data)
            : base($"Application Exception \"{name}\" {data}")
        {
        }
    }
}
