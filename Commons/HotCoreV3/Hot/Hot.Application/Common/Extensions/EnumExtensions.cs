namespace Hot.Application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string? Name<T>(this T data)
            where T : struct, Enum
        {
           return Enum.GetName(typeof(T), data);
        }
    }
}
