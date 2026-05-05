using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherTest
{
    /// <summary>
    /// Статический класс для шифрования и дешифрования строк шифром Хилла.
    /// </summary>
    public static class Encoder
    {
        private static Random random = new Random();

        /// <summary>Алфавит, используемый при кодировании символов в числа.</summary>
        public static string Alphabet { get => _alphabet; }
        private static string _alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя .,;";

        /// <summary>
        /// Проверяет, пригодна ли матрица в качестве ключа (определитель ≠ 0).
        /// </summary>
        public static bool CheckKeyValid(Matrix key)
        {
            return key.GetDeterminant() != 0;
        }

        /// <summary>
        /// Переводит строку в массив числовых кодов по позиции в алфавите.
        /// Символы, отсутствующие в алфавите, получают код -1.
        /// </summary>
        private static int[] ConvertString(string s)
        {
            int[] res = new int[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                int index = _alphabet.IndexOf(s[i]);
                res[i] = index;
            }
            return res;
        }

        /// <summary>
        /// Переводит массив числовых кодов обратно в строку.
        /// Коды берутся по модулю длины алфавита.
        /// </summary>
        private static string ConvertIntArray(int[] array)
        {
            string res = "";
            foreach (int i in array)
            {
                res += _alphabet[((i % _alphabet.Length) + _alphabet.Length) % _alphabet.Length];
            }
            return res;
        }

        /// <summary>
        /// Шифрует строку <paramref name="s"/> матричным ключом шифра Хилла.
        /// Строка разбивается на блоки по ширине ключа; при необходимости дополняется случайными символами.
        /// </summary>
        /// <param name="s">Исходная строка.</param>
        /// <param name="key">Квадратная матрица-ключ.</param>
        /// <returns>Зашифрованная строка, или null если ключ равен null.</returns>
        public static string EncodeString(string s, Matrix key)
        {
            if (s == null) return null;
            string res = "";
            if (key == null) return null;

            int mod = _alphabet.Length;
            int keyWidth = key.Body[0].Length;

            // Дополняем строку до кратной длины блока случайными символами
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

        /// <summary>
        /// Дешифрует строку <paramref name="s"/>, вычисляя обратную матрицу ключа по модулю алфавита.
        /// </summary>
        /// <param name="s">Зашифрованная строка.</param>
        /// <param name="key">Оригинальная матрица-ключ.</param>
        /// <returns>Расшифрованная строка, или null если ключ необратим.</returns>
        public static string DecodeString(string s, Matrix key)
        {
            Matrix inversed = key.GetInverse(_alphabet.Length);
            return EncodeString(s, inversed);
        }
    }
}