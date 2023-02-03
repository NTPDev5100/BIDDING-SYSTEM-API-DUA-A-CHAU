using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public sealed class StringCipher
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("HR$2pIjHR$2pIj12");
        public static string PassPhrase = "P@ssw0rd";

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        public static string Encrypt(string plainText, string passPhrase)
        {
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
                {
                    byte[] keyBytes = password.GetBytes(keysize / 8);
                    using (RijndaelManaged symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    byte[] cipherTextBytes = memoryStream.ToArray();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error in Encryption: Invalid Key. System Error: " + ex.Message.ToString();
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
                {
                    byte[] keyBytes = password.GetBytes(keysize / 8);
                    using (RijndaelManaged symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                        {
                            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //return "Error in Decryption: Invalid Key. System Error: " + ex.Message.ToString(); 
                string error = "Error : " + ex.Message;
                return "";
            }
        }
    }
}
