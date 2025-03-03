namespace Hot.Application.Common.Exceptions
{
    public class HotDbException : Exception
    {
        public HotDbException(string name, object key)
            : base($"\"{name}\":{key}")
        {
        } 
        public HotDbException(string data, Exception error)
      : base($"Database Exception - \"{data}\" - {error.Message}", error)
        {
        }

    }
}
