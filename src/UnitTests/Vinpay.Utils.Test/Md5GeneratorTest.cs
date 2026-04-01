using System.Text;
using Vinpay.Utils.Crypto;

namespace Vinpay.Utils.Test
{
    [TestClass]
    public sealed class Md5GeneratorTest
    {
        private byte[] input =
        [
            123, 45, 189, 78, 211, 9, 156, 87, 234, 56,
            198, 7, 112, 201, 43, 178, 90, 245, 67, 145,
            34, 181, 89, 222, 101, 5, 167, 98, 209, 71,
            132, 29, 190, 81, 241, 62, 150, 11, 173, 84,
            219, 49, 128, 77, 231, 58, 187, 14, 207, 69
        ];

        [TestMethod]
        public void GetMd5BytesFromBytesTest()
        {
            byte[] result =
            [
                0x45, 0x14, 0x9C, 0x63, 0xEC, 0xAA, 0xC3, 0xE7, 0x40, 0xD4, 0x6F, 0xE6, 0xD6, 0x5B, 0xC6, 0x84
            ];

            var output = input.GetMd5Bytes();

            Assert.AreEqual(result.Length, output.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i], output[i]);
            }
        }

        [TestMethod]
        [DataRow("", true, "45149c63ecaac3e740d46fe6d65bc684")]
        [DataRow(" ", true, "45 14 9c 63 ec aa c3 e7 40 d4 6f e6 d6 5b c6 84")]
        [DataRow("", false, "45149C63ECAAC3E740D46FE6D65BC684")]
        [DataRow(" ", false, "45 14 9C 63 EC AA C3 E7 40 D4 6F E6 D6 5B C6 84")]
        public void GetMd5StringFromBytesTest(string separator, bool isLowerCase, string result)
        {
            string output = input.GetMd5String(separator, isLowerCase);
            Assert.AreEqual(result, output);
        }

        [TestMethod]
        public void GetMd5BytesFromStreamTest()
        {
            byte[] result =
            [
                0x45, 0x14, 0x9C, 0x63, 0xEC, 0xAA, 0xC3, 0xE7, 0x40, 0xD4, 0x6F, 0xE6, 0xD6, 0x5B, 0xC6, 0x84
            ];

            MemoryStream stream = new MemoryStream();
            stream.Write(input);
            stream.Seek(0, SeekOrigin.Begin);

            var output = stream.GetMd5Bytes();

            Assert.AreEqual(result.Length, output.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i], output[i]);
            }
        }

        [TestMethod]
        [DataRow("", true, "45149c63ecaac3e740d46fe6d65bc684")]
        [DataRow(" ", true, "45 14 9c 63 ec aa c3 e7 40 d4 6f e6 d6 5b c6 84")]
        [DataRow("", false, "45149C63ECAAC3E740D46FE6D65BC684")]
        [DataRow(" ", false, "45 14 9C 63 EC AA C3 E7 40 D4 6F E6 D6 5B C6 84")]
        public void GetMd5StringFromStreamTest(string separator, bool isLowerCase, string result)
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(input);
            stream.Seek(0, SeekOrigin.Begin);

            var output = stream.GetMd5String(separator, isLowerCase);
            Assert.AreEqual(result, output);
        }

        [TestMethod]
        public void GetMd5BytesFromStringTest()
        {
            byte[] result =
            {
                0x43, 0x7B, 0xBA, 0x8E, 0x0B, 0xF5, 0x83, 0x37, 0x67, 0x4F, 0x45, 0x39, 0xE7, 0x51, 0x86, 0xAC
            };

            string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] output = input.GetMd5Bytes();

            Assert.AreEqual(result.Length, output.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i], output[i]);
            }
        }

        [TestMethod]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "", false, "437BBA8E0BF58337674F4539E75186AC")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", " ", false, "43 7B BA 8E 0B F5 83 37 67 4F 45 39 E7 51 86 AC")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "", true, "437bba8e0bf58337674f4539e75186ac")]
        [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ", " ", true, "43 7b ba 8e 0b f5 83 37 67 4f 45 39 e7 51 86 ac")]
        public void GetMd5StringFromStringTest(string input, string separator, bool isLowerCase, string result)
        {
            string output = input.GetMd5String(Encoding.UTF8, separator, isLowerCase);
            Assert.AreEqual(result, output);
        }

        [TestMethod]
        public void GetMd5BytesFromFileTest()
        {
            byte[] result =
            {
                0x9C, 0x92, 0x57, 0x5C, 0xC2, 0x36, 0x38, 0xAE, 0x5D, 0x9E, 0xD6, 0x93, 0x28, 0x8B, 0x20, 0xC4
            };

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "md5_input.bin");
            byte[] output = filePath.GetFileMd5Bytes();

            Assert.AreEqual(result.Length, output.Length);
            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(result[i], output[i]);
            }
        }

        [TestMethod]
        [DataRow("", false, "9C92575CC23638AE5D9ED693288B20C4")]
        [DataRow(" ", false, "9C 92 57 5C C2 36 38 AE 5D 9E D6 93 28 8B 20 C4")]
        [DataRow("", true, "9c92575cc23638ae5d9ed693288b20c4")]
        [DataRow("-", true, "9c-92-57-5c-c2-36-38-ae-5d-9e-d6-93-28-8b-20-c4")]
        public void GetMd5StringFromFileTest(string separator, bool isLowerCase, string result)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "md5_input.bin");
            string output = filePath.GetFileMd5String(separator, isLowerCase);
            Assert.AreEqual(result, output);
        }
    }
}
