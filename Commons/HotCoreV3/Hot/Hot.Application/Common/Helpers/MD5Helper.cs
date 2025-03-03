using System.Text;

namespace Hot.Application.Common.Helpers;
public static class MD5Helper
{
    public static string ToMD5Hash(this string input)
    {
        using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes); 
        return Convert.ToHexString(hashBytes);
    }
}
