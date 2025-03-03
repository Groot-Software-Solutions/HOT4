namespace Hot.Application.Common.Exceptions;

public class SelfTopupNotAllowedForUserProfileException : Exception
{
    public SelfTopupNotAllowedForUserProfileException(string name, object data)
        : base($"User profile does not allow selftopup \"{name}\" {data}")
    {
    }
    public SelfTopupNotAllowedForUserProfileException(string data, Exception error)
 : base($"$\"User profile does not allow selftopup - \"{data}\" - {error.Message}", error)
    {
    }
}
