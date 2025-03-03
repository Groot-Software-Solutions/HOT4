namespace Hot.BulkRecharge
{
    public static class StringExtensions
    {
        public static  string ToMobileNumber(this string data)
        {
            if (data.StartsWith("+263")) return data.Replace("+263", "0");
            if (data.StartsWith("263")) return $"0{data[3..]}"; 
            if (data.StartsWith("7")) return $"0{data}"; 
            return data;
        }
    }

}
