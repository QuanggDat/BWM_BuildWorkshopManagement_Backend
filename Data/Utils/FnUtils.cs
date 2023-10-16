using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.Utils
{
    public static class FnUtils
    {
        // Kiểm tra ký tự in hoa
        public static bool IsAlphabetOnly(string? val)
        {
            if (string.IsNullOrWhiteSpace(val)) return false;

            // 1 ký tự không thuộc Alphabet (in hoa) => Fail
            return !Regex.IsMatch(val.Trim(), @"[^A-Za-z]+");
        }

        // Kiểm tra số
        public static bool IsNumberOnly(string? val)
        {
            if (string.IsNullOrWhiteSpace(val)) return false;

            // 1 ký tự không thuộc số nguyên => Fail
            return !Regex.IsMatch(val.Trim(), @"[^0-9]+");
        }

        // Kiểm tra số La Mã
        public static bool IsValidRomanNumber(string? val)
        {
            if (string.IsNullOrWhiteSpace(val)) return false;

            var pattern = "^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$";
            return Regex.IsMatch(val.ToUpper().Trim(), pattern);
        }

        // Xoá dấu tiếng Việt
        public static string Remove_VN_Accents(string val)
        {
            if (string.IsNullOrWhiteSpace(val)) return val;

            var normalizedStr = val.Trim().Normalize(NormalizationForm.FormD);
            var strBuilder = new StringBuilder();

            foreach (char c in normalizedStr)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    strBuilder.Append(c);
                }
            }

            return strBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        // Kiểm tra từ đầu tiên trong chuỗi có giống từ cần so sánh không
        public static bool IsFirstWordValid(string inputStr, string conditionWord)
        {
            if (!string.IsNullOrWhiteSpace(inputStr) && inputStr.Length > 0)
            {
                var firstWord = inputStr.Split(' ')[0];
                firstWord = Remove_VN_Accents(firstWord).ToUpper();
                conditionWord = Remove_VN_Accents(conditionWord).ToUpper();
                return firstWord == conditionWord;
            }
            return false;
        }

        // Chuyển string thành int
        public static int ParseStringToInt(string? val, int defaultReturn = 0)
        {
            try
            {
                return Convert.ToInt32(val);
            }
            catch (Exception)
            {
                return defaultReturn;
            }
        }

        // Chuyển string thành double
        public static double ParseStringToDouble(string? val, int roundNum = 1, int defaultReturn = 0)
        {
            try
            {
                var res = Convert.ToDouble(val);
                return Math.Round(res, roundNum);
            }
            catch (Exception)
            {
                return defaultReturn;
            }
        }

        // Tạo mã ngẫu nhiên theo độ dài
        public static string GenerateCode(int len = 6)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var numStr = "0123456789";
            var rd = new Random();
            return string.Join("", Enumerable.Repeat(numStr, len).Select(s => s[rd.Next(s.Length)]).ToList());
        }

        // Chuyển số thành số La Mã
        public static string NumToRomanNum(int num)
        {
            string[] romanNumerals = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            int[] values = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

            string result = string.Empty;

            for (int i = 0; i < values.Length; i++)
            {
                while (num >= values[i])
                {
                    num -= values[i];
                    result += romanNumerals[i];
                }
            }

            return result;
        }

        // Chuyển số thành chữ cái A,B,C,...
        public static string NumToAlphabets(int num)
        {
            string result = "";

            while (num > 0)
            {
                int remainder = (num - 1) % 26;
                char letter = (char)('A' + remainder);
                result = letter + result;
                num = (num - 1) / 26;
            }

            return result;
        }
    }
}
