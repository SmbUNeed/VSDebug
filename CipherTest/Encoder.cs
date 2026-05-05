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
        public static Matrix Key;
        public static string Alphabet {  get => _alphabet; }
        private static string _alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя ";
        //"qwertyuiopasdfghjklzxcvbnmйцукенгшщзфывапролджэячсмитьбю.,?! (){}[]:;-\"
        private static int[] ConvertString(string s)
        {
           

            int[] res = new int[s.Length];
            for(int i = 0; i < s.Length; i++)
            {
                res[i] = _alphabet.IndexOf(s[i]);
            }
            return res;
        }

        private static string ConvertIntArray(int[] array)
        {
            string res = "";
            foreach (int i in array)
            {
                res += _alphabet[i];
            }
            
            return res;
        }

        public static string EncodeString(string s, Matrix key)
        {
            string res = "";
            Key = key;
            if (Key == null) return null;

            int keyWidth = Key.Body[0].Length;
            int toFill = keyWidth - s.Length % keyWidth;

            for (int i = 0; i < toFill; i++)
            {
                s += _alphabet[random.Next(0, _alphabet.Length)];
            }

            for (int i = 0; i < s.Length; i += keyWidth)
            {
                string substring = s.Substring(i, keyWidth);
                int[] converted = ConvertString(substring);


                int[][] m1 = new int[converted.Length][];
                for (int j = 0; j < substring.Length; j++)
                {
                    m1[j] = new int[] { converted[j] };
                }


            }
        }


    }
}
