using System.Globalization;
using Vinpay.Utils.Time;

namespace Vinpay.Utils.Test;

[TestClass]
public class UnixTimestampConverterTest
{
    [TestMethod]
    [DataRow("2026-01-08 19:24:49", 8, 1767871489)]
    [DataRow("1970-01-01 08:00:00", 8, 0)]
    [DataRow("1970-01-01 07:00:00", 7, 0)]
    public void DateTimeToUnixSecondsTest(string dateTimeString, int timeZone, long result)
    {
        string format = "yyyy-MM-dd HH:mm:ss";
        DateTime unspecifiedTime = DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);
        TimeSpan east8Offset = TimeSpan.FromHours(timeZone);
        DateTimeOffset east8TimeOffset = new DateTimeOffset(unspecifiedTime, east8Offset);
        DateTime dateTime = east8TimeOffset.LocalDateTime;

        long unixSeconds = dateTime.ToUnixSeconds();
        Assert.AreEqual(result, unixSeconds);
    }

    [TestMethod]
    [DataRow("1970-01-01 08:00:00 000", 8, 0)]
    [DataRow("1970-01-01 07:00:00 000", 7, 0)]
    [DataRow("1970-01-01 08:00:00 123", 8, 123)]
    [DataRow("2026-01-08 19:24:49 000", 8, 1767871489000)]
    public void DateTimeToUnixMillisecondsTest(string dateTimeString, int timeZone, long result)
    {
        string format = "yyyy-MM-dd HH:mm:ss fff";
        DateTime unspecifiedTime = DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);
        TimeSpan east8Offset = TimeSpan.FromHours(timeZone);
        DateTimeOffset east8TimeOffset = new DateTimeOffset(unspecifiedTime, east8Offset);
        DateTime dateTime = east8TimeOffset.LocalDateTime;

        long unixSeconds = dateTime.ToUnixMilliseconds();
        Assert.AreEqual(result, unixSeconds);
    }

    [TestMethod]
    [DataRow(0, "1970-01-01 00:00:00")]
    [DataRow(1767888000, "2026-01-08 16:00:00")]
    public void UnixSecondsToUtcDateTimeTest(long unixSeconds, string result)
    {
        var utcTime = unixSeconds.UnixTimeSecondsToUtcTime();

        string format = "yyyy-MM-dd HH:mm:ss";
        string dateTimeString = utcTime.ToString(format);
        Assert.AreEqual(result, dateTimeString);
    }

    [TestMethod]
    [DataRow(0, "1970-01-01 00:00:00 000")]
    [DataRow(1767888000123, "2026-01-08 16:00:00 123")]
    public void UnixMillisecondsToUtcDateTimeTest(long unixMilliseconds, string result)
    {
        var utcTime = unixMilliseconds.UnixTimeMillisecondsToUtcTime();

        string format = "yyyy-MM-dd HH:mm:ss fff";
        string dateTimeString = utcTime.ToString(format);
        Assert.AreEqual(result, dateTimeString);
    }

    // The following tests need to be executed in the time zone of Eastern 8th.
    [TestMethod]
    [DataRow(0, "1970-01-01 08:00:00")]
    [DataRow(1767929465, "2026-01-09 11:31:05")]
    public void UnixSecondsToLocalDateTimeTest(long unixSeconds, string result)
    {
        var localTime = unixSeconds.UnixTimeSecondsToLocalTime();

        string format = "yyyy-MM-dd HH:mm:ss";
        string dateTimeString = localTime.ToString(format);
        Assert.AreEqual(result, dateTimeString);
    }

    // The following tests need to be executed in the time zone of Eastern 8th.
    [TestMethod]
    [DataRow(0, "1970-01-01 08:00:00 000")]
    [DataRow(1767871489000, "2026-01-08 19:24:49 000")]
    public void UnixMillisecondsTlocalDateTimeTest(long unixMilliseconds, string result)
    {
        var localTime = unixMilliseconds.UnixTimeMillisecondsToLocalTime();

        string format = "yyyy-MM-dd HH:mm:ss fff";
        string dateTimeString = localTime.ToString(format);
        Assert.AreEqual(result, dateTimeString);
    }
}
