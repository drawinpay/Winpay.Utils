using System.Numerics;

namespace Vinpay.Utils.Data
{
    /// <summary>
    /// A util to convert data size.
    /// </summary>
    public static class DataSizeConverter
    {
        #region Consts

        /// <summary>
        /// One KB
        /// </summary>
        public const double ONE_KB = 1024.0;
        /// <summary>
        /// One MB
        /// </summary>
        public const double ONE_MB = ONE_KB * 1024;
        /// <summary>
        /// One GB
        /// </summary>
        public const double ONE_GB = ONE_MB * 1024;
        /// <summary>
        /// One TB
        /// </summary>
        public const double ONE_TB = ONE_GB * 1024;
        /// <summary>
        /// One PB
        /// </summary>
        public const double ONE_PB = ONE_TB * 1024;

        #endregion

        #region Public Methods

        /// <summary>
        /// Convert the byte length to the length in units of KB 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public static double BytesToKB<T>(this T dataLength) where T : INumber<T>
        {
            var dataSizeInBytes = Convert.ToDouble(dataLength);
            return dataSizeInBytes / ONE_KB;
        }

        /// <summary>
        /// Convert the byte length to the length in units of MB 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public static double BytesToMB<T>(this T dataLength) where T : INumber<T>
        {
            var dataSizeInBytes = Convert.ToDouble(dataLength);
            return dataSizeInBytes / ONE_MB;
        }

        /// <summary>
        /// Convert the byte length to the length in units of GB 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public static double BytesToGB<T>(this T dataLength) where T : INumber<T>
        {
            var dataSizeInBytes = Convert.ToDouble(dataLength);
            return dataSizeInBytes / ONE_GB;
        }

        /// <summary>
        /// Convert the byte length to the length in units of GB 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public static double BytesToTB<T>(this T dataLength) where T : INumber<T>
        {
            var dataSizeInBytes = Convert.ToDouble(dataLength);
            return dataSizeInBytes / ONE_TB;
        }

        /// <summary>
        /// Convert the byte length to the length in units of GB 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public static double BytesToPB<T>(this T dataLength) where T : INumber<T>
        {
            var dataSizeInBytes = Convert.ToDouble(dataLength);
            return dataSizeInBytes / ONE_PB;
        }

        /// <summary>
        /// Data length to string whit unit.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataLength"></param>
        /// <param name="separator"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string ToDataSizeString<T>(this T dataLength, string separator = " ", int digits = 2) where T : INumber<T>
        {
            var dataLengthDouble = Convert.ToDouble(dataLength);
            if (dataLengthDouble < ONE_KB)
            {
                return $"{dataLength}";
            }

            string format = $"{{{0}:F{digits}}}";
            if (dataLengthDouble < ONE_MB)
            {
                return $"{string.Format(format, dataLength.BytesToKB())}{separator}KB";
            }

            if (dataLengthDouble < ONE_GB)
            {
                return $"{string.Format(format, dataLength.BytesToMB())}{separator}MB";
            }

            if (dataLengthDouble < ONE_TB)
            {
                return $"{string.Format(format, dataLength.BytesToGB())}{separator}GB";
            }

            if (dataLengthDouble < ONE_PB)
            {
                return $"{string.Format(format, dataLength.BytesToTB())}{separator}TB";
            }

            return $"{string.Format(format, dataLength.BytesToPB())}{separator}PB";
        }

        #endregion
    }
}
