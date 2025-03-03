using System.Text.RegularExpressions;

namespace Hot.Application.Common.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string data)
        {
            var parsed = int.TryParse(data, out int result);
            return parsed ? result : 0;
        }
        public static decimal ToDecimal(this string data)
        {
            var parsed = decimal.TryParse(data, out decimal result);
            return parsed ? result : 0;
        }
        public static long ToLong(this string data)
        {
            var parsed = long.TryParse(data, out long result);
            return parsed ? result : 0;
        }
        public static bool IsNumeric(this string value) => double.TryParse(value, out _);

        public static int NthIndexOf(this string s, string Character, int NumberOfOccurences)
        {
            var c = Character.ToCharArray()[0];
            var takeCount = s.TakeWhile(x => (NumberOfOccurences -= (x == c ? 1 : 0)) > 0).Count();
            return takeCount == s.Length ? -1 : takeCount;
        }

        public static bool IsValidMobileNumber(this string mobileNumber)
        { 
            var regex = new Regex("(((0|(\\+|)(263))(((7(7|8|1|3)\\d|86(44|77))(\\d{6}))))|((0|(\\+|)(26))(6(\\d{7}))))$");
            return regex.IsMatch(mobileNumber);
        }
    }
}
