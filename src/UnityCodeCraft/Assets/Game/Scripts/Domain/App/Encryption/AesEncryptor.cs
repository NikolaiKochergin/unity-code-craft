using System;
using System.Security.Cryptography;

namespace App.Encryption
{
    public sealed class AesEncryptor : IEncryptor
    {
        private const int KeySize = 32;
        private const int SaltSize = 16;
        private const int IvSize = 16;
        private const int Iterations = 100_000;

        private readonly string _password;

        public AesEncryptor(string password)
        {
            _password = password;
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            byte[] salt = RandomBytes(SaltSize);
            byte[] iv = RandomBytes(IvSize);

            using var pdb = new Rfc2898DeriveBytes(_password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pdb.GetBytes(KeySize);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var encryptor = aes.CreateEncryptor();
            byte[] ciphertext = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Combine(salt, iv, ciphertext);
        }

        public byte[] Decrypt(byte[] input)
        {
            if (input.Length < SaltSize + IvSize)
                throw new Exception("Invalid data");

            int offset = 0;

            byte[] salt = SubArray(input, offset, SaltSize);
            offset += SaltSize;

            byte[] iv = SubArray(input, offset, IvSize);
            offset += IvSize;

            byte[] ciphertext = SubArray(input, offset, input.Length - offset);

            using var pdb = new Rfc2898DeriveBytes(_password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] key = pdb.GetBytes(KeySize);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        }

        private static byte[] RandomBytes(int size)
        {
            byte[] data = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);
            return data;
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            int length = 0;
            foreach (var arr in arrays)
                length += arr.Length;

            byte[] result = new byte[length];

            int offset = 0;
            foreach (var arr in arrays)
            {
                Buffer.BlockCopy(arr, 0, result, offset, arr.Length);
                offset += arr.Length;
            }

            return result;
        }

        private static byte[] SubArray(byte[] data, int index, int length)
        {
            byte[] result = new byte[length];
            Buffer.BlockCopy(data, index, result, 0, length);
            return result;
        }
    }
}