﻿using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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
        public static string GenerateSalt(long accessID)
        {
            using (MD5 md5 = MD5.Create())
            {
                string input = accessID.ToString();
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString.Substring(0, 20); // Substring for first 20 characters
            }
        }

        public static string GeneratePasswordHash(string salt, string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                string input = (salt ?? "") + password;
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        public static bool CheckValidEmail(string email)
        {
            string emailText = email?.Trim() ?? string.Empty;

            // Check if email is empty
            if (string.IsNullOrEmpty(emailText))
                return false;

            // Check if email contains spaces
            if (emailText.Contains(" "))
                return false;

            // Check if email contains invalid characters
            if (emailText.Any(c => "(),:;<>\\".Contains(c)))
                return false;

            // Check if domain part contains invalid special characters
            int atIndex = emailText.IndexOf('@');
            if (atIndex >= 0)
            {
                string domainPart = emailText.Substring(atIndex);
                if (domainPart.Any(c => "!#$%&*+/=?^`_{|}".Contains(c)))
                    return false;
            }

            // Check if the first or last character is one of these special characters
            if (emailText.FirstOrDefault() is char first && "-_.+".Contains(first))
                return false;

            if (emailText.LastOrDefault() is char last && "-_.+".Contains(last))
                return false;

            // Check if email contains square brackets
            if (emailText.Contains("[") || emailText.Contains("]"))
                return false;

            // Check if email contains multiple '@' symbols
            if (emailText.Count(c => c == '@') > 1)
                return false;

            // Check if email does not match the expected pattern (user@domain.extension)
            var emailPattern = @"^[^@]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            if (!Regex.IsMatch(emailText, emailPattern))
                return false;

            // If none of the checks failed, it's a valid email
            return true;
        }
    }
}
