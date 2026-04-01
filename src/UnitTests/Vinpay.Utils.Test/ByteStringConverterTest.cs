using Vinpay.Utils.Data;

namespace Vinpay.Utils.Test
{
    [TestClass]
    public class ByteStringConverterTest
    {
        [TestMethod]
        [DataRow("45149C63ECAAC3E740D46FE6D65BC684")]
        [DataRow("45149c63ecaac3e740d46fe6d65bc684")]
        public void ParseHexStringTest(string input)
        {

            byte[] result =
            [
                0x45, 0x14, 0x9C, 0x63, 0xEC, 0xAA, 0xC3, 0xE7, 0x40, 0xD4, 0x6F, 0xE6, 0xD6, 0x5B, 0xC6, 0x84
            ];

            byte[] output = ByteStringConverter.ParseHexString(input);

            Assert.AreEqual(result.Length, output.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i], output[i]);
            }
        }

        [TestMethod]
        [DataRow("45149C63ECAAC3E740D46FE6D65BC6841")]
        [DataRow("45149c63ecaac3e740d46fe6d65bc6841")]
        public void ParseHexStringThrowException(string input)
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                byte[] output = ByteStringConverter.ParseHexString(input);
            });
        }
        [TestMethod]
        [DataRow("45 14 9C 63 EC AA C3 E7 40 D4 6F E6 D6 5B C6 84", " ")]
        [DataRow("45 14 9c 63 ec aa c3 e7 40 d4 6f e6 d6 5b c6 84", " ")]
        [DataRow("0x45 0x14 0x9c 0x63 0xec 0xaa 0xc3 0xe7 0x40 0xd4 0x6f 0xe6 0xd6 0x5b 0xc6 0x84", " ")]
        [DataRow("0x45, 0x14, 0x9c, 0x63, 0xec, 0xaa, 0xc3, 0xe7, 0x40, 0xd4, 0x6f, 0xe6, 0xd6, 0x5b, 0xc6, 0x84", ",")]
        public void ParseHexStringTest(string input, string separator)
        {

            byte[] result =
            [
                0x45, 0x14, 0x9C, 0x63, 0xEC, 0xAA, 0xC3, 0xE7, 0x40, 0xD4, 0x6F, 0xE6, 0xD6, 0x5B, 0xC6, 0x84
            ];

            byte[] output = ByteStringConverter.ParseHexString(input, separator);

            Assert.AreEqual(result.Length, output.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i], output[i]);
            }
        }

        [TestMethod]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, " ", true, "35 47 b5 97 ce")]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, "", true, "3547b597ce")]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, "-", true, "35-47-b5-97-ce")]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, " 0x", true, "35 0x47 0xb5 0x97 0xce")]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, " ", false, "35 47 B5 97 CE")]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, "", false, "3547B597CE")]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, "-", false, "35-47-B5-97-CE")]
        [DataRow(new byte[] { 0x35, 0x47, 0xB5, 0x97, 0xCE }, " 0x", false, "35 0x47 0xB5 0x97 0xCE")]
        [DataRow(new byte[] { 53, 71, 181, 151, 206 }, " ", true, "35 47 b5 97 ce")]
        [DataRow(new byte[] { 53, 71, 181, 151, 206 }, " ", false, "35 47 B5 97 CE")]
        public void BytesToHexStringTest(byte[] data, string separator, bool isLowerCase, string result)
        {
            string output = data.BytesToHexString(separator, isLowerCase);
            Assert.AreEqual(result, output);
        }
    }
}
