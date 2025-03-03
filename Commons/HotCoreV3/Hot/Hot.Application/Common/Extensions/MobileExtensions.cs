namespace Hot.Application.Common.Extensions
{
    public static class MobileExtensions
    {
        public static string ToMSISDN (this string mobile)
        {
            var result = mobile;
            if (result.StartsWith("0")) result = $"263{result[1..]}";
            if (result.StartsWith("+")) result = $"{result[1..]}"; 
            if (result.StartsWith("7")) result = $"263{result}";
            if (result.StartsWith("6")) result = $"266{result}";
            return result;
        }

        public static string ToMobile(this string mobile)
        {
            var result = mobile; 
            if (result.StartsWith("+")) result = $"{result[1..]}";
            if (result.StartsWith("263")) result = $"0{result[3..]}";
            if (result.StartsWith("7")) result = $"0{result}";
            if (result.StartsWith("266")) result = $"{result[3..]}";
            if (result.StartsWith("06")) result = $"{result[1..]}";
            return result;
        }
      
    }
}
