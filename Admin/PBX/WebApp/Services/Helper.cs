using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Services
{
    public static class Helper
    {
        public static string FormatTime(DateTime date)
        { 
            TimeSpan ts = DateTime.Now.Subtract(date); 
            return ts.Days switch
            {
                _ when (DateTime.Now.Day != date.Day) && (ts.Days <= 1) => "yesterday", 
                0 => ts.Hours switch {
                    < 1 => $"{ts.Minutes} mins ago",
                    < 2 => $"hour ago",
                    < 4 => $"{ts.Hours} hours ago",
                    _ => date.ToString("h:mm tt")
                }, 
                > 1 and < 4 => date.DayOfWeek.ToString(),
                _ => date.ToString("M/dd/yy")
            };
        }

        public static string FormatMobile(string input)
        {
            if ((input ?? "").Length < 10) return input;
            return $"{input.Substring(0, 4)}{input.Substring(4, 3)}{input.Substring(7, 3)}";
        }
    }
}
