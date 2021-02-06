using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CelloStore.Utilities.Extensions.String
{
    public static class StringExtension
    {
        /// <summary>
        /// Convert any string expression to numeric or it's default value (zero).
        /// </summary>
        /// <typeparam name="T">Generic return data type</typeparam>
        /// <param name="s">string expression to be evaluated</param>
        /// <returns>numeric or zero value</returns>
        public static T ToDefaultNumber<T>(this System.String s) where T : struct
        {
            if (s.Trim().Equals(System.String.Empty))
                return default(T);

            return (T)Convert.ChangeType(s, typeof(T));
        }

        public static DateTime AsDateTime(this string s)
        {
            return DateTime.Parse(s, Thread.CurrentThread.CurrentCulture);
        }

        public static string EmptyIfNull(this string s)
        {
            return s == null ? System.String.Empty : s;
        }

        public static string Left(this string s, int charCount)
        {
            return s.Substring(0, charCount);
        }

        public static string Right(this string s, int charCount)
        {
            return s.Substring(s.Length - charCount);
        }

        /// <summary>
        /// true, if is valid email address
        /// from http://www.davidhayden.com/blog/dave/
        /// archive/2006/11/30/ExtensionMethodsCSharp.aspx
        /// </summary>
        /// <param name="s">email address to test</param>
        /// <returns>true, if is valid email address</returns>
        public static bool IsValidEmailAddress(this string s)
        {
            return new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,6}$").IsMatch(s);
        }

        /// <summary>
        /// Checks if url is valid. 
        /// from http://www.osix.net/modules/article/?id=586 and changed to match http://localhost
        /// 
        /// complete (not only http) url regex can be found 
        /// at http://internet.ls-la.net/folklore/url-regexpr.html
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidUrl(this string url)
        {
            string strRegex = "^(https?://)"
        + "?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?" //user@
        + @"(([0-9]{1,3}\.){3}[0-9]{1,3}" // IP- 199.194.52.184
        + "|" // allows either IP or domain
        + @"([0-9a-z_!~*'()-]+\.)*" // tertiary domain(s)- www.
        + @"([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]" // second level domain
        + @"(\.[a-z]{2,6})?)" // first level domain- .com or .museum is optional
        + "(:[0-9]{1,5})?" // port number- :80
        + "((/?)|" // a slash isn't required if there is no file name
        + "(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";
            return new Regex(strRegex).IsMatch(url);
        }
        

        /// <summary>
        /// Reverse the string
        /// from http://en.wikipedia.org/wiki/Extension_method
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Reverse(this string input)
        {
            char[] chars = input.ToCharArray();
            Array.Reverse(chars);
            return new System.String(chars);
        }


        /// <summary>
        /// Reduce string to shorter preview which is optionally ended by some string (...).
        /// </summary>
        /// <param name="s">string to reduce</param>
        /// <param name="count">Length of returned string including endings.</param>
        /// <param name="endings">optional edings of reduced text</param>
        /// <example>
        /// string description = "This is very long description of something";
        /// string preview = description.Reduce(20,"...");
        /// produce -> "This is very long..."
        /// </example>
        /// <returns></returns>
        public static string Reduce(this string s, int count, string endings)
        {
            if (count < endings.Length)
                throw new Exception("Failed to reduce to less then endings length.");
            int sLength = s.Length;
            int len = sLength;
            if (endings != null)
                len += endings.Length;
            if (count > sLength)
                return s; //it's too short to reduce
            s = s.Substring(0, sLength - len + count);
            if (endings != null)
                s += endings;
            return s;
        }

        /// <summary>
        /// remove white space, not line end
        /// Useful when parsing user input such phone,
        /// price int.Parse("1 000 000".RemoveSpaces(),.....
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value">string without spaces</param>
        public static string RemoveSpaces(this string s)
        {
            return s.Replace(" ", "");
        }

        /// <summary>
        /// true, if the string can be parse as Double respective Int32
        /// Spaces are not considred.
        /// </summary>
        /// <param name="s">input string</param>
        /// <param name="floatpoint">true, if Double is considered,
        /// otherwhise Int32 is considered.</param>
        /// <returns>true, if the string contains only digits or float-point</returns>
        public static bool IsNumber(this string s, bool floatpoint)
        {
            int i;
            double d;
            string withoutWhiteSpace = s.RemoveSpaces();
            if (floatpoint)
                return double.TryParse(withoutWhiteSpace, NumberStyles.Any,
                    Thread.CurrentThread.CurrentUICulture, out d);
            else
                return int.TryParse(withoutWhiteSpace, out i);
        }

        /// <summary>
        /// true, if the string contains only digits or float-point.
        /// Spaces are not considred.
        /// </summary>
        /// <param name="s">input string</param>
        /// <param name="floatpoint">true, if float-point is considered</param>
        /// <returns>true, if the string contains only digits or float-point</returns>
        public static bool IsNumberOnly(this string s, bool floatpoint)
        {
            s = s.Trim();
            if (s.Length == 0)
                return false;
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                {
                    if (floatpoint && (c == '.' || c == ','))
                        continue;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Remove accent from strings 
        /// </summary>
        /// <example>
        ///  input:  "Příliš žluťoučký kůň úpěl ďábelské ódy."
        ///  result: "Prilis zlutoucky kun upel dabelske ody."
        /// </example>
        /// <param name="s"></param>
        /// <remarks>founded at http://stackoverflow.com/questions/249087/
        /// how-do-i-remove-diacritics-accents-from-a-string-in-net</remarks>
        /// <returns>string without accents</returns>
        public static string RemoveDiacritics(this string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Replace \r\n or \n by <br />
        /// from http://weblogs.asp.net/gunnarpeipman/archive/2007/11/18/c-extension-methods.aspx
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ConvertNewlineToBR(this string s)
        {
            return s.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }

        /// <summary>
        /// Returns the given string truncated to the specified length, suffixed with an elipses (...)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">Maximum length of return string</param>
        /// <returns></returns>
        public static string Truncate(this string input, int length)
        {
            return Truncate(input, length, "...");
        }

        /// <summary>
        /// Returns the given string truncated to the specified length, suffixed with the given value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length">Maximum length of return string</param>
        /// <param name="suffix">The value to suffix the return value with (if truncation is performed)</param>
        /// <returns></returns>
        public static string Truncate(this string input, int length, string suffix)
        {
            if (input == null) return "";
            if (input.Length <= length) return input;

            if (suffix == null) suffix = "...";

            return input.Substring(0, length - suffix.Length) + suffix;
        }

        /// <summary>
        /// Splits a given string into an array based on character line breaks
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String array, each containing one line</returns>
        public static string[] ToLineArray(this string input)
        {
            if (input == null) return new string[] { };
            return System.Text.RegularExpressions.Regex.Split(input, "\r\n");
        }

        /// <summary>
        /// Splits a given string into a strongly-typed list based on character line breaks
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Strongly-typed string list, each containing one line</returns>
        public static List<string> ToLineList(this string input)
        {
            List<string> output = new List<string>();
            output.AddRange(input.ToLineArray());
            return output;
        }

        /// <summary>
        /// Replaces line breaks with self-closing HTML 'br' tags
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceBreaksWithBR(this string input)
        {
            return string.Join("<br/>", input.ToLineArray());
        }

        /// <summary>
        /// Replaces any single apostrophes with two of the same
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string DoubleApostrophes(this string input)
        {
            return Regex.Replace(input, "'", "''");
        }        

        /// <summary>
        /// Removes any HTML tags from the input string
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String</returns>
        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, @"<(style|script)[^<>]*>.*?</\1>|</?[a-z][a-z0-9]*[^<>]*>|<!--.*?-->", "");
        }

        public static string Xor(this string clearText, int xorAmount)
        {
            char[] resultText = clearText.ToCharArray();
            for (int index = 0; index < clearText.Length; index++)
            {
                resultText[index] = (char)(clearText[index] ^ xorAmount);
            }

            return new System.String(resultText);

        }

        public static string RandomLetters(this string input, int letters)
        {
            for(int i = 1; i <= letters; i++)
            {
                Random _random = new Random();
                int num = _random.Next(0, 26); // Zero to 25
                char let = (char)('a' + num);
                input += let;                
            }

            return input;
        }

        public static string GetMd5Hash(this string input)
        {
            var md5 = MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
