namespace Hot.Application.Common.Exceptions;

public class FailedToRegisterUserException : Exception
{
    public FailedToRegisterUserException(string name, object data)
        : base($"Failed to register user in the system \"{name}\" {data}")
    {
    }
    public FailedToRegisterUserException(string data, Exception error)
 : base($"Failed to register user in the system - \"{data}\" - {error.Message}", error)
    {
    }
}
