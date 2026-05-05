using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTest
{
    internal static class Encoder
    {
        private static Random random = new Random();
        public static string Alphabet {  get => _alphabet; }
        private static string _alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя .,;";

        public static bool CheckKeyValid(Matrix key)
        {
            return key.GetDeterminant() != 0;
        }

        private static int[] ConvertString(string s)
        {
            int[] res = new int[s.Length];
            for(int i = 0; i < s.Length; i++)
            {
                int index = _alphabet.IndexOf(s[i]);
                res[i] = index;
            }
            return res;
        }

        private static string ConvertIntArray(int[] array)
        {
            string res = "";
            foreach (int i in array)
            {
                res += _alphabet[((i % _alphabet.Length) + _alphabet.Length) % _alphabet.Length];
            }
            return res;
        }

        public static string EncodeString(string s, Matrix key)
        {
            string res = "";
            if (key == null) return null;

            int mod = _alphabet.Length;
            int keyWidth = key.Body[0].Length;

            int toFill = (keyWidth - s.Length % keyWidth) % keyWidth;
            for (int i = 0; i < toFill; i++)
                s += _alphabet[random.Next(0, mod)];

            for (int i = 0; i < s.Length; i += keyWidth)
            {
                string substring = s.Substring(i, keyWidth);
                int[] converted = ConvertString(substring);
                res += ConvertIntArray(Matrix.MultiplyWithKey(key.Body, converted, mod));
            }

            return res;
        }

        public static string DecodeString(string s, Matrix key)
        {
            Matrix inversed = key.GetInverse(_alphabet.Length);
            return EncodeString(s, inversed);
        }
    }
}
