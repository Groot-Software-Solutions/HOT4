
namespace Hot.Application.Common.Exceptions;

public class ReservationNotFoundException :Exception
{
    public ReservationNotFoundException(string name, object data)
    : base($"Account Not Found Exception - \"{name}\" {data}")
    {
        Data.Add("ObjectData", data);
        Data.Add("Message", name);
    }
    public ReservationNotFoundException(string data, Exception error)
        : base($"Account Not Found Exception - \"{data}\" - {error.Message}", error)
    {
    }
    public ReservationNotFoundException(string message)
       : base($"{message}")
    {
    }
}
