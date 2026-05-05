using CipherTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HillCipherTestProject
{
    [TestClass]
    public class HillCipherTests
    {
        // ─── Вспомогательные матрицы ───────────────────────────────────────────

        /// <summary>2×2, det=11, НОД(11,38)=1 — обратима.</summary>
        private static Matrix Key2x2 => new Matrix(new[]
        {
            new[] { 3, 2 },
            new[] { 5, 7 }
        });

        /// <summary>2×2, det=11 — второй ключ для TC_SEC_02.</summary>
        private static Matrix Key2x2Alt => new Matrix(new[]
        {
            new[] { 1, 2 },
            new[] { 3, 5 }
        });

        /// <summary>3×3, det=1, НОД(1,38)=1 — обратима.</summary>
        private static Matrix Key3x3 => new Matrix(new[]
        {
            new[] { 1, 2, 3 },
            new[] { 0, 1, 4 },
            new[] { 5, 6, 0 }
        });

        /// <summary>2×2, det=0 — вырожденная (TC_FUNC_06).</summary>
        private static Matrix KeySingular => new Matrix(new[]
        {
            new[] { 2, 4 },
            new[] { 1, 2 }
        });

        /// <summary>2×2, det=2, НОД(2,38)=2>1 — необратима по mod 38 (TC_FUNC_07).</summary>
        private static Matrix KeyNotCoprime => new Matrix(new[]
        {
            new[] { 37, 1 },
            new[] { 0,  1 }
        });

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_01 | Шифрование корректного текста матрицей 2×2
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_01 — Шифртекст отличается от открытого, длина сохраняется")]
        public void TC_FUNC_01_Encrypt_2x2_ResultDiffersFromOriginal()
        {
            string original = "привет";
            string encrypted = Encoder.EncodeString(original, Key2x2);

            Assert.IsNotNull(encrypted, "EncodeString не должен возвращать null для корректных данных");
            Assert.AreEqual(original.Length, encrypted.Length, "Длина шифртекста должна совпадать с длиной открытого текста (без дополнения)");
            Assert.AreNotEqual(original, encrypted, "Шифртекст не должен совпадать с открытым текстом");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_02 | Дешифрование зашифрованного текста матрицей 2×2
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_02 — Decode(Encode(s)) == s для текста без дополнения")]
        public void TC_FUNC_02_DecryptAfterEncrypt_2x2_ReturnsOriginal()
        {
            string original = "привет"; // 6 символов, кратно 2 — дополнения нет
            string encrypted = Encoder.EncodeString(original, Key2x2);
            string decrypted = Encoder.DecodeString(encrypted, Key2x2);

            Assert.IsNotNull(decrypted, "DecodeString не должен возвращать null");
            Assert.AreEqual(original, decrypted, "Дешифрованный текст должен совпадать с исходным");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_03 | Шифрование корректного текста матрицей 3×3
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_03 — Шифртекст имеет длину кратную 3 и отличается от открытого")]
        public void TC_FUNC_03_Encrypt_3x3_ResultDiffersAndLengthMultipleOf3()
        {
            string original = "абвгдеж"; // 7 символов — дополнится до 9
            string encrypted = Encoder.EncodeString(original, Key3x3);

            Assert.IsNotNull(encrypted);
            Assert.AreEqual(0, encrypted.Length % 3, "Длина шифртекста должна быть кратна 3");
            Assert.AreNotEqual(original, encrypted.Substring(0, original.Length), "Шифртекст не должен начинаться с открытого текста");

            // Все символы шифртекста входят в алфавит
            foreach (char c in encrypted)
                Assert.IsTrue(Encoder.Alphabet.IndexOf(c) >= 0, $"Символ '{c}' не входит в алфавит");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_04 | Цикл шифрование-дешифрование матрицей 3×3
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_04 — Decode(Encode(s)) начинается с s при дополнении блока")]
        public void TC_FUNC_04_DecryptAfterEncrypt_3x3_StartsWithOriginal()
        {
            string original = "солнцеярко"; // 10 символов, дополнится до 12
            string encrypted = Encoder.EncodeString(original, Key3x3);
            string decrypted = Encoder.DecodeString(encrypted, Key3x3);

            Assert.IsNotNull(decrypted);
            Assert.IsTrue(decrypted.StartsWith(original),
                $"Дешифрованный текст должен начинаться с '{original}', получено: '{decrypted}'");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_05 | Проверка детерминанта обратимой матрицы
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_05 — GetDeterminant возвращает 11, CheckKeyValid == true")]
        public void TC_FUNC_05_ValidKey_DeterminantAndCheckKeyValid()
        {
            Matrix key = Key2x2;

            int det = key.GetDeterminant();
            bool isValid = Encoder.CheckKeyValid(key);

            Assert.AreEqual(11, det, "det([[3,2],[5,7]]) должен быть равен 11");
            Assert.IsTrue(isValid, "CheckKeyValid должен вернуть true для обратимой матрицы");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_06 | Отклонение вырожденной матрицы (det = 0)
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_06 — GetInverse вырожденной матрицы возвращает null")]
        public void TC_FUNC_06_SingularMatrix_GetInverseReturnsNull()
        {
            Matrix key = KeySingular; // det = 0

            Assert.AreEqual(0, key.GetDeterminant(), "det([[2,4],[1,2]]) должен быть 0");
            Assert.IsFalse(Encoder.CheckKeyValid(key), "CheckKeyValid должен вернуть false");
            Assert.IsNull(key.GetInverse(Encoder.Alphabet.Length), "GetInverse вырожденной матрицы должен вернуть null");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_07 | Отклонение матрицы с det, не взаимно простым с |алфавита|
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_07 — GetInverse матрицы с det кратным 37 возвращает null")]
        public void TC_FUNC_07_KeyWithDetNotCoprimeToMod_GetInverseReturnsNull()
        {
            // det([[37,1],[0,1]]) = 37, НОД(37, 37) = 37 > 1 — необратима
            // Алфавит длиной 37 (простое число) — единственный способ сломать
            // обратимость: det должен быть кратен 37
            Matrix key = KeyNotCoprime;

            Assert.AreEqual(37, key.GetDeterminant());
            Assert.IsNull(key.GetInverse(Encoder.Alphabet.Length),
                "GetInverse должен вернуть null, если det кратен размеру алфавита");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_08 | Шифрование текста нечётной длины (дополнение блока 2×2)
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_08 — Текст нечётной длины дополняется до кратности 2")]
        public void TC_FUNC_08_OddLengthText_PaddedToEvenLength()
        {
            string original = "приветмир"; // 9 символов
            string encrypted = Encoder.EncodeString(original, Key2x2);

            Assert.IsNotNull(encrypted);
            Assert.AreEqual(10, encrypted.Length, "Шифртекст нечётного текста должен иметь длину 10");
            Assert.AreEqual(0, encrypted.Length % 2, "Длина шифртекста должна быть чётной");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_FUNC_09 | Дополнение до кратности 3 при матрице 3×3
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_FUNC_09 — Текст длиной 2 дополняется до 3, длиной 3 — без дополнения")]
        public void TC_FUNC_09_3x3_PaddingToMultipleOf3()
        {
            string text2 = "аа";  // 2 символа → дополнить до 3
            string text3 = "ааб"; // 3 символа → без дополнения

            string enc2 = Encoder.EncodeString(text2, Key3x3);
            string enc3 = Encoder.EncodeString(text3, Key3x3);

            Assert.IsNotNull(enc2);
            Assert.IsNotNull(enc3);
            Assert.AreEqual(3, enc2.Length, "Текст 'аа' должен зашифроваться в 3 символа после дополнения");
            Assert.AreEqual(3, enc3.Length, "Текст 'ааб' уже кратен 3, длина шифртекста = 3");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_NEG_01 | Шифрование пустой строки
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_NEG_01 — Пустая строка шифруется в пустую строку без исключений")]
        public void TC_NEG_01_EmptyString_ReturnsEmptyWithoutCrash()
        {
            string result = Encoder.EncodeString("", Key2x2);

            // Модуль шифрования не падает и возвращает пустую строку
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result, "Шифрование пустой строки должно возвращать пустую строку");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_NEG_02 | Передача null в модуль шифрования
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_NEG_02 — EncodeString(null, key) возвращает null")]
        public void TC_NEG_02_NullText_ThrowsException()
        {
            // null-строка должна вернуть null
            Assert.IsNull(Encoder.EncodeString(null, Key2x2));
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_NEG_03 | Символы вне алфавита дают индекс -1
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_NEG_03 — Символы вне алфавита попадают в шифртекст как мусор (индекс -1 от алфавита)")]
        public void TC_NEG_03_TextWithForeignChars_ProducesResultButNotMeaningful()
        {
            // Латинские символы и цифры не входят в алфавит — IndexOf вернёт -1.
            // Текущий код не выбрасывает исключение, но результат некорректен.
            // Тест фиксирует текущее поведение (нет краша).
            string text = "privet";
            string result = Encoder.EncodeString(text, Key2x2);

            Assert.IsNotNull(result, "Код не должен падать при символах вне алфавита");
            Assert.AreEqual(text.Length, result.Length, "Длина результата совпадает с длиной входа");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_SEC_01 | Шифртекст значимо отличается от открытого
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_SEC_01 — Диффузия: повторяющийся открытый текст ≠ открытый текст")]
        public void TC_SEC_01_RepeatingPlaintext_EncryptedDiffersFromOriginal()
        {
            string text1 = "бббббббббббб"; // 12 символов 'а'
            string text2 = "абабабабабаб"; // 12 символов чередование

            string enc1 = Encoder.EncodeString(text1, Key2x2);
            string enc2 = Encoder.EncodeString(text2, Key2x2);

            Assert.AreNotEqual(text1, enc1, "Шифртекст не должен совпадать с открытым текстом (текст 1)");
            Assert.AreNotEqual(text2, enc2, "Шифртекст не должен совпадать с открытым текстом (текст 2)");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_SEC_02 | Разные ключи → разные шифртексты
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Description("TC_SEC_02 — Один открытый текст, два ключа → два разных шифртекста")]
        public void TC_SEC_02_DifferentKeys_ProduceDifferentCiphertexts()
        {
            string original = "привет";

            string enc1 = Encoder.EncodeString(original, Key2x2);
            string enc2 = Encoder.EncodeString(original, Key2x2Alt);

            Assert.IsNotNull(enc1);
            Assert.IsNotNull(enc2);
            Assert.AreNotEqual(enc1, enc2, "Разные ключи должны давать разные шифртексты");
        }

        // ──────────────────────────────────────────────────────────────────────
        // TC_UI_01 / TC_UI_02 / TC_UI_03 — ручное тестирование
        // ──────────────────────────────────────────────────────────────────────
        [TestMethod]
        [Ignore("TC_UI_01 — ручной тест: проверка наличия Tooltip у элементов интерфейса")]
        public void TC_UI_01_TooltipsExistForAllControls() { }

        [TestMethod]
        [Ignore("TC_UI_02 — ручной тест: кнопка 'Очистить' сбрасывает все поля")]
        public void TC_UI_02_ClearButton_ResetsAllFields() { }

        [TestMethod]
        [Ignore("TC_UI_03 — ручной тест: переключение 2×2 / 3×3 обновляет форму матрицы")]
        public void TC_UI_03_MatrixSizeSwitch_UpdatesForm() { }
    }
}