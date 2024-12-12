using System.Security.Cryptography;
using System.Text;

namespace Hot4.Core.Helper
{
    public static class Helper
    {
        public static string CreateRandomSMSAccessCode()
        {
            Random random = new Random();
            var rnd = random.Next(1, 9999);
            return rnd.ToString().PadLeft(4, '0');
        }

        public static string ToMD5Hash(string input)
        {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToHexString(hashBytes);
        }
    }
}
