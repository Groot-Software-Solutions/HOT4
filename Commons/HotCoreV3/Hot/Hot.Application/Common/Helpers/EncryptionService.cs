using System.Security.Cryptography;

namespace Hot.Application.Common.Helpers
{

    public static class EncryptionService
    {

        public static string EncryptStringAES(string plainText, string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                throw new ArgumentNullException(plainText);
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(password);
            }

            AesCryptoServiceProvider aesAlg = new();
            string retval = null;
            try
            {

                Rfc2898DeriveBytes key = new(password, salt);
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using MemoryStream msEncrypt = new ();
                msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using StreamWriter swEncrypt = new(csEncrypt);
                    swEncrypt.Write(plainText);
                }
                retval = Convert.ToBase64String(msEncrypt.ToArray());
            }
            finally
            {
                aesAlg?.Clear();
            }
            return retval;

        }

        public static string DecryptStringAES(string cipherText, string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                throw new ArgumentNullException(cipherText);
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(password);
            }

            AesCryptoServiceProvider aesAlg = new ();
            string retval = null;

            try
            {
                Rfc2898DeriveBytes key = new (password, salt);

                byte[] bytes = Convert.FromBase64String(cipherText);
                using MemoryStream msDecrypt = new(bytes);

                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = ReadByteArray(msDecrypt);
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);
                retval = srDecrypt.ReadToEnd();
            }
            finally
            {
                aesAlg?.Clear();
            }

            return retval;
        }

        public static string EncryptStringRC4(string plainText, string password, string salt)
        {
            return RC4.Encrypt(password + salt, plainText);
        }

        public static string DecryptStringRC4(string plainText, string password, string salt)
        {
            return RC4.Decrypt(password + salt, plainText);
        }

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        public static string EncryptStringRC4(string pinNumber, object encryptionKey, object pinReference)
        {
            throw new NotImplementedException();
        }
    }
}
