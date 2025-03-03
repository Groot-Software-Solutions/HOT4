using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Ecocash.Application.Common.Exceptions
{
    public class HotEcocashAppException : Exception
    {
        public HotEcocashAppException(string name, object data)
            : base($"Application Exception \"{name}\" {data}")
        {
        }
    }
}
