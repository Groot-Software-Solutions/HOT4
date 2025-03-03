namespace Hot.Application.Common.Exceptions
{
    public class APIException : Exception
    {
        public APIException(string name, object data)
            : base($"API Endpoint Exception \"{name}\" {data}")
        {
        }
        public APIException(string data, Exception error)
      : base($"API Endpoint Exception - \"{data}\" - {error.Message}", error)
        {
        }
    } 
}
