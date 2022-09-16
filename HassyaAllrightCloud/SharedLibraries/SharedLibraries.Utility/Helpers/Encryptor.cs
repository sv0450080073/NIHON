using SharedLibraries.Utility.Exceptions;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SharedLibraries.Utility.Helpers
{
    public static class Encryptor
    {
        public static string Key = "key";
        public static string Encrypt(string text)
        {
            try
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    using (var tdes = new TripleDESCryptoServiceProvider())
                    {
                        tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
                        tdes.Mode = CipherMode.ECB;
                        tdes.Padding = PaddingMode.PKCS7;

                        using (var transform = tdes.CreateEncryptor())
                        {
                            byte[] textBytes = Encoding.UTF8.GetBytes(text);
                            byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                            return Convert.ToBase64String(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new EncryptException(e.ToString());
            }
        }

        public static string Decrypt(string cipher)
        {
            try
            {
                using (var md5 = new MD5CryptoServiceProvider())
                {
                    using (var tdes = new TripleDESCryptoServiceProvider())
                    {
                        tdes.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(Key));
                        tdes.Mode = CipherMode.ECB;
                        tdes.Padding = PaddingMode.PKCS7;

                        using (var transform = tdes.CreateDecryptor())
                        {
                            byte[] cipherBytes = Convert.FromBase64String(cipher);
                            byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                            return Encoding.UTF8.GetString(bytes);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new DecryptException(e.ToString());
            }
        }
    }
}
