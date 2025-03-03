namespace Hot.Application.Common.Exceptions;
public class NotFoundException : Exception
{
    public NotFoundException(string name, object data)
        : base($"Not Found Exception \"{name}\" {data}")
    {
        Data.Add("ObjectData", data);
        Data.Add("Message", name);
    }
    public NotFoundException(string data, Exception error)
 : base($"Not Found Exception - \"{data}\" - {error.Message}", error)
    {
    }

    public NotFoundException(string message)
       : base($"{message}")
    { 
    } 
}
