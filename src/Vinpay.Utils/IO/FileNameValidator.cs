using System.Text;

namespace Vinpay.Utils.IO
{
    /// <summary>
    /// A util to deal with file name issues.
    /// </summary>
    public static class FileNameValidator
    {
        private static readonly char[] IllegalStrings =
        {
            '\\', '/', ':', '*', '?', '"', '<', '>', '|'
        };

        /// <summary>
        /// Determin whether a string contains any illegal characters.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsLegal(this string fileName)
        {
            foreach (var subStr in fileName)
            {
                if (IllegalStrings.Contains(subStr))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Correct the illegal characters in a file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="alter"></param>
        /// <returns></returns>
        public static string CorrectIllegalCharacters(this string fileName, string alter = "")
        {
            bool isReplaceIllegalChar = !string.IsNullOrEmpty(alter);
            var stringBuilder = new StringBuilder(fileName);
            if (!isReplaceIllegalChar)
            {
                for (int i = fileName.Length - 1; i >= 0; i--)
                {
                    if (IllegalStrings.Contains(stringBuilder[i]))
                    {
                        stringBuilder.Remove(i, 1);
                    }
                }
            }
            else
            {
                for (int i = fileName.Length - 1; i >= 0; i--)
                {
                    if (IllegalStrings.Contains(stringBuilder[i]))
                    {
                        stringBuilder.Remove(i, 1);
                        stringBuilder.Insert(i, alter);
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }
}
