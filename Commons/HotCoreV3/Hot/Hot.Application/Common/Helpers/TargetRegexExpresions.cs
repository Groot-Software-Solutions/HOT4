
using System.Text.RegularExpressions;
namespace Hot.Application.Common.Helpers;
public partial class TargetRegexExpresions
{
    [GeneratedRegex("^(0|((\\+|)263)|)7(8|7)\\d{7}$")]
    public static partial Regex EconetMobileNumberRegex();

    [GeneratedRegex("^(0|((\\+|)263)|)71\\d{7}$")]
    public static partial Regex NetoneMobileNumberRegex();

    [GeneratedRegex("^(0|((\\+|)263)|)73\\d{7}$")]
    public static partial Regex TelecelMobileNumberRegex();

    [GeneratedRegex("^(0|((\\+|)266)|)6\\d{7}$")]
    public static partial Regex LesothoMobileNumberRegex();

    [GeneratedRegex("^(0|((\\+|)(263|266))|)(7\\d|6)\\d{7}$")]
    public static partial Regex MobileNumberRegex();

    [GeneratedRegex("^\\d{11}$")]
    public static partial Regex ZESAMeterNumberRegex();

    [GeneratedRegex("^(0|)([2-6]\\d{2})\\d{6}$")]
    public static partial Regex TeloneNumberRegex();

}