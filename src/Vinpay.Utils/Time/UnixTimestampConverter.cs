namespace Vinpay.Utils.Time
{
    /// <summary>
    /// A util to convert data between DateTime and unit timestamp.
    /// </summary>
    public static class UnixTimestampConverter
    {
        /// <summary>
        /// Convert DateTime to unix time seconds.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixSeconds(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUniversalTime().ToUnixTimeSeconds();
        }

        /// <summary>
        /// Convert DateTime to unix time milliseconds.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixMilliseconds(this DateTime dateTime)
        {
            return new DateTimeOffset(dateTime).ToUniversalTime().ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Convert unix time seconds to utc DateTime.
        /// </summary>
        /// <param name="unixTimeSeconds"></param>
        /// <returns></returns>
        public static DateTime UnixTimeSecondsToUtcTime(this long unixTimeSeconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds).UtcDateTime;
        }

        /// <summary>
        /// Convert unix time seconds to local DateTime
        /// </summary>
        /// <param name="unixTimeSeconds"></param>
        /// <returns></returns>
        public static DateTime UnixTimeSecondsToLocalTime(this long unixTimeSeconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeSeconds).LocalDateTime;
        }

        /// <summary>
        /// Convert unix time milliseconds to utc DateTime.
        /// </summary>
        /// <param name="unixTimeMilliseconds"></param>
        /// <returns></returns>
        public static DateTime UnixTimeMillisecondsToUtcTime(this long unixTimeMilliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds).UtcDateTime;
        }

        /// <summary>
        /// Convert unix time milliseconds to local DateTime.
        /// </summary>
        /// <param name="unixTimeMilliseconds"></param>
        /// <returns></returns>
        public static DateTime UnixTimeMillisecondsToLocalTime(this long unixTimeMilliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTimeMilliseconds).LocalDateTime;
        }
    }
}
