namespace Hot.Application.Common.Extensions
{
    public static class UnixTimeExtensions
    {
        public static DateTime ToDateTimeFromUnixTime(this long unixtime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(unixtime);
        }
    }
}
