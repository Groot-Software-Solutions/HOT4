
namespace Hot.Application.Common.Exceptions;

public class NetworkProviderException : Exception
{
    public NetworkProviderException(string name, object data)
     : base($"Network Provider Exception - \"{name}\" {data}")
    {
    }
    public NetworkProviderException(string data, Exception error)
      : base($"Network Provider Exception - \"{data}\" - {error.Message}", error)
    {
    }
}
