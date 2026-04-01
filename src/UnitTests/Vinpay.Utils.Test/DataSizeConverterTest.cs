using Vinpay.Utils.Data;

namespace Vinpay.Utils.Test
{
    [TestClass]
    public class DataSizeConverterTest
    {
        [TestMethod]
        [DataRow(1024, 1)]
        [DataRow(13917, 13.590820)]
        public void BytesToKBTest(long dataLenght, double result)
        {
            var dataSizeInKB = dataLenght.BytesToKB();
            Assert.IsTrue(Math.Abs(result - dataSizeInKB) < 1E-6);
        }

        [TestMethod]
        [DataRow(1048576, 1)]
        [DataRow(9065736, 8.645759)]
        public void BytesToMBTest(long dataLenght, double result)
        {
            var dataSizeInMB = dataLenght.BytesToMB();
            Assert.IsTrue(Math.Abs(result - dataSizeInMB) < 1E-6);
        }

        [TestMethod]
        [DataRow(1073741824, 1)]
        [DataRow(1342177280, 1.25)]
        public void BytesToGBTest(long dataLength, double result)
        {
            var dataSizeInGB = dataLength.BytesToGB();
            Assert.IsTrue(Math.Abs(result - dataSizeInGB) < 1E-6);
        }

        [TestMethod]
        [DataRow(1099511627776, 1)]
        [DataRow(2051064174825, 1.865432)]
        public void BytesToTBTest(long dataLength, double result)
        {
            var dataSizeInTB = dataLength.BytesToTB();
            Assert.IsTrue(Math.Abs(result - dataSizeInTB) < 1E-6);
        }

        [TestMethod]
        [DataRow(1125899906842624, 1)]
        [DataRow(1429892881690132, 1.27)]
        public void BytesToPBTest(long dataLength, double result)
        {
            var dataSizeInPB = dataLength.BytesToPB();
            Assert.IsTrue(Math.Abs(result - dataSizeInPB) < 1E-6);
        }

        [TestMethod]

        [DataRow(13917, " ", 2, "13.59 KB")]
        [DataRow(13917, "", 2, "13.59KB")]
        [DataRow(13917, " ", 1, "13.6 KB")]
        [DataRow(9065736, " ", 2, "8.65 MB")]
        [DataRow(9065736, "", 2, "8.65MB")]
        [DataRow(9065736, " ", 1, "8.6 MB")]
        [DataRow(1342177280, " ", 2, "1.25 GB")]
        [DataRow(1342177280, "", 2, "1.25GB")]
        [DataRow(1342177280, " ", 1, "1.2 GB")]
        [DataRow(2051064174825, " ", 2, "1.87 TB")]
        [DataRow(2051064174825, "", 2, "1.87TB")]
        [DataRow(2051064174825, " ", 1, "1.9 TB")]
        [DataRow(1429892881690132, " ", 2, "1.27 PB")]
        [DataRow(1429892881690132, "", 2, "1.27PB")]
        [DataRow(1429892881690132, " ", 1, "1.3 PB")]
        public void ToDataSizeStringTest(long dataLength, string separator, int digits, string result)
        {
            string output = dataLength.ToDataSizeString(separator, digits);
            Assert.AreEqual(result, output);
        }
    }
}
