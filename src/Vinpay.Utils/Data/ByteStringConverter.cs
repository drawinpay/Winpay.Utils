using System.Text;

namespace Vinpay.Utils.Data
{
    /// <summary>
    /// Parse binary data from strings.
    /// </summary>
    public static class ByteStringConverter
    {
        /// <summary>
        /// Parse hex string to byte array.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] ParseHexString(this string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("The length of the input hexadecimal string is not an even number.");
            }

            byte[] buffer = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return buffer;
        }

        /// <summary>
        /// Parse hex string to byte array.
        /// </summary>
        /// <param name="hexString">Input hex string</param>
        /// <param name="separator">Separator between each hex byte string.</param>
        /// <returns></returns>
        public static byte[] ParseHexString(this string hexString, string separator)
        {
            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentNullException(nameof(separator));
            }

            var hexStringArray = hexString.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            byte[] buffer = new byte[hexStringArray.Length];
            for (int i = 0; i < hexStringArray.Length; i++)
            {
                buffer[i] = Convert.ToByte(hexStringArray[i].Trim(), 16);
            }

            return buffer;
        }

        /// <summary>
        /// Convert the byte array to a hexadecimal string
        /// </summary>
        /// <param name="data">The input byte array.</param>
        /// <param name="separator">The separator between each byte.</param>
        /// <param name="isLowerCase">Indicates whether the MD5 string is in lowercase</param>
        /// <returns></returns>
        public static string BytesToHexString(this byte[] data, string separator = "", bool isLowerCase = true)
        {
            StringBuilder sb = new StringBuilder();
            string format = isLowerCase ? "x2" : "X2";
            bool useSeparator = !string.IsNullOrEmpty(separator);

            if (useSeparator)
            {
                for (int i = 0; i < data.Length - 1; i++)
                {
                    sb.Append(data[i].ToString(format));
                    sb.Append(separator);
                }
                sb.Append(data[data.Length - 1].ToString(format));
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {

                    sb.Append(data[i].ToString(format));
                }
            }

            return sb.ToString();
        }
    }
}
