using System;
using System.Text;

namespace App.Encryption
{
    public static class Extensions
    {
        public static string Encrypt(this IEncryptor encryptor, string input)
        {
            byte[] convertedInput = Encoding.UTF8.GetBytes(input);
            byte[] encryptedInput = encryptor.Encrypt(convertedInput);
            return Convert.ToBase64String(encryptedInput);
        }
        
        public static string Decrypt(this IEncryptor encryptor, string input)
        {
            byte[] convertedInput = Convert.FromBase64String(input);
            byte[] decryptedInput = encryptor.Decrypt(convertedInput);
            return Encoding.UTF8.GetString(decryptedInput);
        }
    }
}