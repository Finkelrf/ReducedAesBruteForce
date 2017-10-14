using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace aesBruteForce
{
    class Aes
    {
        public static byte[] decrypt(byte[] input, byte[] key)
        {
            var aesAlg = new AesManaged
            {
                KeySize = 128,
                Key = key,
                BlockSize = 128,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros,
                IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            ICryptoTransform encryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            return encryptor.TransformFinalBlock(input, 0, input.Length);
        }

        public static byte[] encrypt(byte[] input, byte[] key)
        {
            var aesAlg = new AesManaged
            {
                KeySize = 128,
                Key = key,
                BlockSize = 128,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.Zeros,
                IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            return encryptor.TransformFinalBlock(input, 0, input.Length);
        }

        internal static byte[] incrementKey(byte[] key)
        {
            return (new BigInteger(key)+1).ToByteArray();
        }

        public static byte[] getNextValidKey(byte[] keyBytes)
        {
            bool carry = false;
            int i;
            for (i = keyBytes.Length - 1; i >= 0; i--)
            {
                if (carry)
                {
                    int keyByte = keyBytes[i];
                    keyByte++;
                    keyBytes[i] = (byte)keyByte;
                    if (keyBytes[i] != 0)
                    {
                        carry = false;
                    }
                }
                if (keyBytes[i] < 33)
                {
                    keyBytes[i] = (byte)(33);
                }
                else if (keyBytes[i] > 126)
                {
                    keyBytes[i] = 33;
                    carry = true;
                }
            }
            return keyBytes;
        }
    }
}
