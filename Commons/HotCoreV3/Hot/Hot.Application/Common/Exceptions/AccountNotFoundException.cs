
namespace Hot.Application.Common.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string name, object data)
     : base($"Account Not Found Exception - \"{name}\" {data}")
    {
        Data.Add("ObjectData", data);
        Data.Add("Message", name);
    }
    public AccountNotFoundException(string data, Exception error)
        : base($"Account Not Found Exception - \"{data}\" - {error.Message}", error)
    {
    }
    public AccountNotFoundException(string message)
       : base($"{message}")
    {
    }
}
