using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace EncryptedSantaAssigner
{
    public static class ILIstUtilities
    {
        private static readonly Random rnd = new Random();

        /// <summary>
        /// Shuffles the element order of the specified list, using a Fisher-Yates shuffle.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            int count = ts.Count;
            for (var i = 0; i < count - 1; ++i)
            {
                int r = rnd.Next(i, count);
                ts.Swap(i, r);
            }
        }

        /// <summary>
        /// Swap elements at positions i and r in a given list.
        /// </summary>
        public static void Swap<T>(this IList<T> ts, int i, int r)
        {
            T tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }

        /// <summary>
        /// Shuffles the element order of the specified list.
        /// This method doesn't allow any element to stay in its initial position if the list is of size > 1 (derangement).
        /// </summary>
        public static void DerangementShuffle<T>(this IList<T> ts)
        {
            int count = ts.Count;
            if (count < 2)
                return;

            bool[] thisElementWasSwappedAlready = new bool[count];
            thisElementWasSwappedAlready.Populate(false);

            for (var i = 0; i < count - 2; ++i)
            {
                int swapStartIndex;
                if (thisElementWasSwappedAlready[i])
                {
                    swapStartIndex = i; // If the element at i is already at a swapped position, leave a chance for this iteration to not change its position, as this would prevent some derangements from potentially being reached.
                }
                else
                {
                    swapStartIndex = i + 1;
                }

                int r = rnd.Next(swapStartIndex, count);
                ts.Swap(i, r);

                thisElementWasSwappedAlready[i] = true;
                thisElementWasSwappedAlready[r] = true;
            }

            // All elements were deranged, except the last 2 ones that might or might not have been.
            // If one of these two wasn't swapped, swapping them is the only way to obtain a derangement at this point.
            if(!thisElementWasSwappedAlready[count - 1] || !thisElementWasSwappedAlready[count - 2])
            {
                ts.Swap(count - 1, count - 2);
            }
            // If both were, we could leave it as that, but swapping them will also be a correct result (ie a derangement) by construction.
            // To ensure all derangements are attainable, we'll do a 50/50 toss
            else
            {
                bool shouldSwap = (rnd.Next(0,2) == 0); // rnd.Next(0,2) returns 0 or 1
                if (shouldSwap)
                {
                    ts.Swap(count - 1, count - 2);
                }
            }
        }

        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }
    }

    public static class EncryptionUtilities
    {
        public static string ToEncryptedString(string cipherText, RSACryptoServiceProvider rsa)
        {
            AesManaged aes = new AesManaged();

            // Write the following to the result:
            // - length of the key
            // - length of the IV
            // - ecrypted key
            // - the IV
            // - the encrypted cipher content
            byte[] keyEncrypted = rsa.Encrypt(aes.Key, false); // We can't just put the Aes key in plain bytes in the result, so we encrypt it using asymmetric RSA keys first.

            byte[] LenK = BitConverter.GetBytes(keyEncrypted.Length);
            byte[] LenIV = BitConverter.GetBytes(aes.IV.Length);
            byte[] encryptedCipher = Encrypt(cipherText, aes.Key, aes.IV);

            List<byte> encryptedData = new List<byte>();
            encryptedData.AddRange(LenK);
            encryptedData.AddRange(LenIV);
            encryptedData.AddRange(keyEncrypted);
            encryptedData.AddRange(aes.IV);
            encryptedData.AddRange(encryptedCipher);

            return Convert.ToBase64String(encryptedData.ToArray());
        }

        private static byte[] Encrypt(this string cipherText, byte[] Key, byte[] IV)
        {
            byte[] encryptedCipher;

            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Key, IV);
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create crypto stream using the CryptoStream class. This class is the key to encryption    
                    // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream    
                    // to encrypt    
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(cipherText);
                        encryptedCipher = ms.ToArray();
                    }
                }
            }

            return encryptedCipher;
        }
    }
}
