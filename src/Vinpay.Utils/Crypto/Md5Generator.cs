using System.Security.Cryptography;
using System.Text;
using Vinpay.Utils.Data;

namespace Vinpay.Utils.Crypto
{
    /// <summary>
    /// A util to calculate MD5 hash.
    /// </summary>
    public static class Md5Generator
    {
        #region Public Methods

        /// <summary>
        /// Calculate the MD5 value of a byte array.
        /// </summary>
        /// <param name="input">A byte array.</param>
        /// <returns>A byte array for the MD5 value</returns>
        public static byte[] GetMd5Bytes(this byte[] input)
        {
            using var md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(input);
            return hashBytes;
        }

        /// <summary>
        /// Calculate the MD5 value of a string.
        /// </summary>
        /// <param name="input">A string to calculate the MD5 value.</param>
        /// <param name="encoding">The encoding of the input string.</param>
        /// <returns>A byte array for the MD5 value</returns>
        public static byte[] GetMd5Bytes(this string input, Encoding? encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            byte[] inputBytes = encoding.GetBytes(input);
            return inputBytes.GetMd5Bytes();
        }

        /// <summary>
        /// Calculate the MD5 value of a stream.
        /// </summary>
        /// <param name="input">The input stream to calculate MD5 value</param>
        /// <returns>A byte array for the MD5 value</returns>
        public static byte[] GetMd5Bytes(this Stream input)
        {
            using var md5 = MD5.Create();
            byte[] hashBytes = md5.ComputeHash(input);
            return hashBytes;
        }

        /// <summary>
        /// Calculate the MD5 value of a file.
        /// </summary>
        /// <param name="filePath">The file path for calculating the MD5 value</param>
        /// <returns>A byte array for the MD5 value</returns>
        public static byte[] GetFileMd5Bytes(this string filePath)
        {
            using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return fileStream.GetMd5Bytes();
        }

        /// <summary>
        /// Calculate the MD5 string of a byte array.
        /// </summary>
        /// <param name="input">A byte array.</param>
        /// <param name="separator">The separator string between each byte.</param>
        /// <param name="isLowerCase">Indicates whether the MD5 string is in lowercase</param>
        /// <returns>A string for the MD5 value</returns>
        public static string GetMd5String(this byte[] input, string separator = "", bool isLowerCase = true)
        {
            var hashBytes = input.GetMd5Bytes();
            var hashString = hashBytes.BytesToHexString(separator, isLowerCase);
            return hashString;
        }

        /// <summary>
        /// Calculate the MD5 string of a string.
        /// </summary>
        /// <param name="input">A string to calculate the MD5 string.</param>
        /// <param name="encoding">The encoding of the input string.</param>
        /// <param name="separator">The separator string between each byte.</param>
        /// <param name="isLowerCase">Indicates whether the MD5 string is in lowercase</param>
        /// <returns>A string for the MD5 value</returns>
        public static string GetMd5String(this string input, Encoding? encoding = null, string separator = "", bool isLowerCase = true)
        {
            var hashBytes = input.GetMd5Bytes(encoding);
            var hashString = hashBytes.BytesToHexString(separator, isLowerCase);
            return hashString;
        }

        /// <summary>
        /// Calculate the MD5 string of a string.
        /// </summary>
        /// <param name="stream">The input stream to calculate MD5 value</param>
        /// <param name="separator">The separator string between each byte.</param>
        /// <param name="isLowerCase">Indicates whether the MD5 string is in lowercase</param>
        /// <returns>A string for the MD5 value</returns>
        public static string GetMd5String(this Stream stream, string separator = "", bool isLowerCase = true)
        {
            var hashBytes = stream.GetMd5Bytes();
            var hashString = hashBytes.BytesToHexString(separator, isLowerCase);
            return hashString;
        }

        /// <summary>
        /// Calculate the MD5 string of a file.
        /// </summary>
        /// <param name="filePath">The file path for calculating the MD5 value</param>
        /// <param name="separator">The separator string between each byte.</param>
        /// <param name="isLowerCase">Indicates whether the MD5 string is in lowercase</param>
        /// <returns>A byte array for the MD5 value</returns>
        public static string GetFileMd5String(this string filePath, string separator = "", bool isLowerCase = true)
        {
            var hashBytes = filePath.GetFileMd5Bytes();
            var hashString = hashBytes.BytesToHexString(separator, isLowerCase);
            return hashString;
        }

        #endregion
    }
}
