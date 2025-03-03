
namespace Hot.Application.Common.Exceptions;
public class AccountDisabledException : Exception
{
    public AccountDisabledException(string name, object data)
     : base($"Account Disabled Exception - \"{name}\" {data}")
    {
    }
    public AccountDisabledException(string data, Exception error)
        : base($"Account Disabled Exception - \"{data}\" - {error.Message}",error)
        {
        }
}

