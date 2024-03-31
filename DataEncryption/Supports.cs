using System.Security.Cryptography;
using System.Text;

namespace DataEncryption
{
    public static class Supports
    {
        public static string GenerateMD5(string input)
        {
            using (MD5  md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }

        public static string DecryptAES(string cipherText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(cipherText);
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes("PSVJQRk9QTEpNVU1DWUZCRVFGV1VVT0=");
                aes.IV = Encoding.UTF8.GetBytes("2314345645678765"); //2314345645678765
                ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] secret = crypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                crypto.Dispose();
                return ASCIIEncoding.ASCII.GetString(secret);
            }
        }
    }
}
