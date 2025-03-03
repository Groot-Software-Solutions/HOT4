namespace Hot.Application.Common.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string name, object data)
            : base($"Application Exception \"{name}\" {data}")
        {
        }
        public AppException(string data, Exception error)
      : base($"Application Exception - \"{data}\" - {error.Message}", error)
        {
        }

        public int LogError()
        {
            throw new NotImplementedException();
        }
    }
}
