using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Application.Common.Exceptions
{
    public class MobileWalletPaymentException: Exception
    {
        public MobileWalletPaymentException(string name, object key)
          : base($"\"{name}\":{key}")
        {
        }
        public MobileWalletPaymentException(string data, Exception error)
      : base($"Database Exception - \"{data}\" - {error.Message}", error)
        {
        }
    }
}
