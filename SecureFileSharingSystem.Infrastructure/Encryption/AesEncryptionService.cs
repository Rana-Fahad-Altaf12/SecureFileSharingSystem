using System.Security.Cryptography;
using System.Text;

namespace SecureFileSharingSystem.Infrastructure.Encryption
{
    public static class AesEncryptionService
    {
        public static byte[] Encrypt(byte[] data, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);

            // Prepend IV for decryption
            var result = new byte[aes.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

            return result;
        }

        public static byte[] Decrypt(byte[] data, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);

            // Extract IV
            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] ciphertext = new byte[data.Length - iv.Length];
            Buffer.BlockCopy(data, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(data, iv.Length, ciphertext, 0, ciphertext.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        }
    }
}