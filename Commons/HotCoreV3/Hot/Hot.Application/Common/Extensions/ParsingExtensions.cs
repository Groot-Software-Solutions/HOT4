using System.Diagnostics.CodeAnalysis;

namespace Hot.Application.Common.Extensions;
public static class ParsingExtensions
{
    public static T Parse<T>(this string value, IFormatProvider? formatProvider = null)
        where T : IParsable<T>
    {
        return T.Parse(value, formatProvider);
    }
    public static bool TryParse<T>(this string value, [MaybeNullWhen(false)]out T? result)
        where T : IParsable<T>
    {
        return T.TryParse(value,null ,out result);
    }

}
