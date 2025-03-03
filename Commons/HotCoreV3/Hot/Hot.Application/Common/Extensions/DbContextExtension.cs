namespace Hot.Application.Common.Extensions
{
    public static class DbContextExtension
    {
        public static T? ResultOrNull<T>(this OneOf<T?, HotDbException> response) where T : class
        {
            return response.Match(
                result => result,
                error => { return default; }
                );
        }
        public static int ResultOrNull(this OneOf<int, HotDbException> response)
        {
            return response.Match(
                result => result,
                error => { return -1; }
                );
        }
        public static decimal ResultOrNull(this OneOf<decimal, HotDbException> response)
        {
            return response.Match(
                result => result,
                error => { return -1; }
                );
        }
        public static long ResultOrNull(this OneOf<long, HotDbException> response)
        {
            return response.Match(
                result => result,
                error => { return -1; }
                );
        }
        public static bool ResultOrNull(this OneOf<bool, HotDbException> response)
        {
            return response.Match(
                result => result,
                error => { return false; }
                );
        }
        public static bool IsNotFoundException(this HotDbException dbException)
        {
            return dbException.Message.Contains("Sequence contains no elements");
        } 

    }
}
