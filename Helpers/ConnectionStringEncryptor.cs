using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EversisZadanieRekrutacyjne.Helpers
{
    public static class ConnectionStringEncryptor
    {
        private static readonly string Password;

        static ConnectionStringEncryptor()
        {
            // Odczytaj hasło z pliku konfiguracyjnego app.config
            Password = ConfigurationManager.AppSettings["EncryptionPassword"];
        }

        public static string EncryptConnectionString(string connectionString)
        {
            byte[] salt = GenerateSalt();
            byte[] key = GenerateKey(salt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.GenerateIV();

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] connectionBytes = Encoding.UTF8.GetBytes(connectionString);
                        cs.Write(connectionBytes, 0, connectionBytes.Length);
                    }

                    byte[] encryptedBytes = ms.ToArray();
                    byte[] encryptedData = new byte[salt.Length + encryptedBytes.Length];

                    Buffer.BlockCopy(salt, 0, encryptedData, 0, salt.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, encryptedData, salt.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(encryptedData);
                }
            }
        }

        public static string DecryptConnectionString(string encryptedConnectionString)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedConnectionString);
            byte[] salt = new byte[16];
            byte[] encryptedBytes = new byte[encryptedData.Length - salt.Length];

            Buffer.BlockCopy(encryptedData, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(encryptedData, salt.Length, encryptedBytes, 0, encryptedBytes.Length);

            byte[] key = GenerateKey(salt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                byte[] iv = new byte[aes.BlockSize / 8];
                Buffer.BlockCopy(encryptedBytes, 0, iv, 0, iv.Length);
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            string decryptedString = sr.ReadToEnd();
                            return decryptedString;
                        }
                    }
                }
            }
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        private static byte[] GenerateKey(byte[] salt)
        {
            const int Iterations = 10000;
            const int KeySize = 32; // 256 bits

            using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(Password, salt, Iterations))
            {
                return rfc2898.GetBytes(KeySize);
            }
        }
    }
}