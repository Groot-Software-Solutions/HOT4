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
        public static string GenerateSalt(long id)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(id.ToString()));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower().Substring(0, 20);
            }
        }
        public static string GeneratePasswordHash(string salt, string newPassword)
        {
            string combined = (salt ?? "") + newPassword;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public static string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var byteValue in hashBytes)
                {
                    sb.Append(byteValue.ToString("x2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}
