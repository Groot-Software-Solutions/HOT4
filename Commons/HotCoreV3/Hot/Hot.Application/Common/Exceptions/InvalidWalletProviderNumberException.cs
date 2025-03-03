namespace Hot.Application.Common.Exceptions;

public class InvalidWalletProviderNumberException : Exception
{
    public InvalidWalletProviderNumberException(string name, object data)
        : base($"User profile does not allow selftopup \"{name}\" {data}")
    {
    }
    public InvalidWalletProviderNumberException(string data, Exception error)
 : base($"$\"User profile does not allow selftopup - \"{data}\" - {error.Message}", error)
    {
    }
}
