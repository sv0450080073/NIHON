using System;
using System.Linq;
using HassyaAllrightCloud.Commons.Helpers;

namespace HassyaAllrightCloud.Commons.Extensions
{
    public static class EncryptExtensions
    {
        public static int[] GetExchanges(int size, int key)
        {
            int[] exchanges = new int[size - 1];
            var rand = new Random(key);
            for (int i = size - 1; i > 0; i--)
            {
                int n = rand.Next(i + 1);
                exchanges[size - 1 - i] = n;
            }
            return exchanges;
        }

        public static string EncryptString(this string input, int key)
        {
            var compressed = StringCompressor.CompressString(input);
            int size = compressed.Length;
            char[] chars = compressed.ToArray();
            var exchanges = GetExchanges(size, key);
            for (int i = size - 1; i > 0; i--)
            {
                int n = exchanges[size - 1 - i];
                char tmp = chars[i];
                chars[i] = chars[n];
                chars[n] = tmp;
            }
            return new string(chars);
        }

        public static string DecryptString(this string encrypted, int key)
        {
            int size = encrypted.Length;
            char[] chars = encrypted.ToArray();
            var exchanges = GetExchanges(size, key);
            for (int i = 1; i < size; i++)
            {
                int n = exchanges[size - i - 1];
                char tmp = chars[i];
                chars[i] = chars[n];
                chars[n] = tmp;
            }
            var compressed = new string(chars);
            return StringCompressor.DecompressString(compressed);
        }
    }
}