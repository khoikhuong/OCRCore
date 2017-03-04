using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCRCore.Common.Helper
{
    public static class AppExtensions
    {
        static string[] DATE_FORMAT = new string[] { "MM/dd/yyyy", "M/dd/yyyy", "MM/d/yyyy", "M/d/yyyy", "yyyy,M,d",
                                                     "yyyy,MM,dd", "yyyy,MM,d", "yyyy,M,dd", "MMddyy", "Mddyy" };

        /// <summary>
        /// Chunked list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Chunked<T>(this List<T> source, int chunkSize)
        {
            int offset = 0;
            while (offset < source.Count)
            {
                yield return source.GetRange(offset, Math.Min(source.Count - offset, chunkSize));
                offset += chunkSize;
            }
        }

        public static DateTime? ToDate(this string value, params string[] format)
        {
            DateTime? result = null;
            try
            {
                if (format == null || format.Length == 0)
                {
                    result = DateTime.ParseExact(value, DATE_FORMAT, null, System.Globalization.DateTimeStyles.None);
                }
                else
                {
                    result = DateTime.ParseExact(value, format, null, System.Globalization.DateTimeStyles.None);
                }
            }
            catch
            {
                //Console.Write(ex);
            }
            return result;
        }

        /// <summary>
        /// Truncates the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Left(this string input, int maxLength)
        {
            if (input.Length <= maxLength) return input.Trim();

            string output = input.Substring(0, maxLength);
            return output.Trim();
        }

        /// <summary>
        /// Gets the right side of the string.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string str, int length)
        {
            if (str.Length <= length) return str;
            return str.Substring(str.Length - length);
        }

        /// <summary>
        /// Determines if a string consists of all valid ASCII values.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsAscii(this string str)
        {
            foreach (var ch in str)
                if ((int)ch > 127) return false;

            return true;
        }

        public static int CountBreakLine(this string str)
        {
            int counter = 1;
            string[] strTemp = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            if (strTemp.Length > 0)
            {
                counter = strTemp.Length;
            }
            return counter;
        }

        public static bool IsValidOneWord(this string input)
        {
            string word = input.Trim().ToLower();
            if (!String.IsNullOrEmpty(word))
            {
                foreach (char c in word)
                {
                    if (char.IsLetter(c) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
