using System.Text.RegularExpressions;

namespace Hot.Application.Common.Extensions;
public static class IDNumberExtensions
{
    public static bool IsValidIDNumber(this string  IdNumber)
    {
         return Regex.IsMatch(IdNumber.Trim(), "^\\d{2}( |-|)\\d{6}\\d?( |-|)[a-zA-Z]( |-|)\\d{2}$"); 
    }
}
