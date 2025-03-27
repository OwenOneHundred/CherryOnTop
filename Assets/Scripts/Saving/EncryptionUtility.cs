using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSaves
{
    public class EncryptionUtility
    {

        private static string key = "thisisacoolencryptionkey:)";
        private static byte encryptionFlag = 0b1;

        public static string DecryptFile(byte[] rawData, out byte flags)
        {
            flags = rawData[0];
            byte[] textBytes = new byte[rawData.Length - 1];
            Buffer.BlockCopy(rawData, 1, textBytes, 0, textBytes.Length);
            bool encrypted = (flags & encryptionFlag) == 0;
#if UNITY_EDITOR
            // Inside of a directive to prevent game builds from avoiding encryption
            // If it's not encrypted, just using the UTF8 encoding!
            if (!encrypted)
            {
                return Encoding.UTF8.GetString(textBytes);
            }
#endif
            string textData;


            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.Key = keyBytes;

                // Ensure the text is long enough to contain an IV
                int ivSize = aesAlg.BlockSize / 8; // 16 bytes for AES
                if (textBytes.Length < ivSize)
                {
                    Debug.LogError("Ciphertext too short to contain a valid IV, decryption aborted.");
                    return null;
                }

                byte[] iv = new byte[ivSize];
                Buffer.BlockCopy(textBytes, 0, iv, 0, ivSize);
                aesAlg.IV = iv;

                int encryptedDataLength = textBytes.Length - ivSize;
                if (encryptedDataLength <= 0)
                {
                    Debug.LogError("No encrypted data found after IV.");
                    return null;
                }

                byte[] encryptedData = new byte[encryptedDataLength];
                Buffer.BlockCopy(textBytes, ivSize, encryptedData, 0, encryptedDataLength);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
            return textData;
        }

        public static byte[] EncryptFile(string rawData, bool useEncryption = true)
        {
            byte flags = (byte)0;
            byte[] data;
#if UNITY_EDITOR
            // Inside of a directive to prevent game builds from avoiding encryption
            // If we're not encrypting it, just use the UTF8 encoding.
            if (!useEncryption)
            {
                byte[] originalData = Encoding.UTF8.GetBytes(rawData);
                data = new byte[1 + originalData.Length];
                flags |= encryptionFlag;
                data[0] = flags;
                Buffer.BlockCopy(originalData, 0, data, 1, originalData.Length);
                return data;
            }
#endif
            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(rawData);
                    }
                    data = msEncrypt.ToArray();
                }
            }
            byte[] finalData = new byte[data.Length + 1];
            finalData[0] = flags;
            Buffer.BlockCopy(data, 0, finalData, 1, data.Length);
            return finalData;
        }
    }
}