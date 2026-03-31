using Vinpay.Utils.Data;

namespace Vinpay.Utils.Test;

[TestClass]
public class BitReaderTest
{
    [TestMethod]
    public void Constructor_WithoutOffset_SetsDataAndDefaultOffset()
    {
        byte[] data = [0xD6, 0x69];

        var reader = new BitReader(data);

        Assert.AreSame(data, reader.Data);
        Assert.AreEqual(0, reader.ByteOffset);
    }

    [TestMethod]
    public void Constructor_WithOffset_SetsDataAndOffset()
    {
        byte[] data = [0x00, 0xD6, 0x69];

        var reader = new BitReader(data, 1);

        Assert.AreSame(data, reader.Data);
        Assert.AreEqual(1, reader.ByteOffset);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_WithNegativeOffset_ThrowsException()
    {
        byte[] data = [0xD6, 0x69];

        _ = new BitReader(data, -1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_WithOffsetEqualToLength_ThrowsException()
    {
        byte[] data = [0xD6, 0x69];

        _ = new BitReader(data, data.Length);
    }

    [TestMethod]
    [DataRow(0, 0)]
    [DataRow(1, 1)]
    [DataRow(2, 1)]
    [DataRow(3, 0)]
    [DataRow(4, 1)]
    [DataRow(5, 0)]
    [DataRow(6, 1)]
    [DataRow(7, 1)]
    public void ReadBit_ReturnsExpectedBit(int bitIndex, int expected)
    {
        byte[] data = [0xD6]; // 11010110

        var reader = new BitReader(data);

        int result = reader.ReadBit(0, bitIndex);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ReadBitAsBool_ReturnsExpectedBoolean()
    {
        byte[] data = [0xD6]; // 11010110

        var reader = new BitReader(data);

        Assert.IsTrue(reader.ReadBitAsBool(0, 1));
        Assert.IsFalse(reader.ReadBitAsBool(0, 0));
    }

    [TestMethod]
    public void ReadBit_WithByteOffset_ReturnsExpectedBit()
    {
        byte[] data = [0x00, 0xD6, 0x69];

        var reader = new BitReader(data, 1);

        int result = reader.ReadBit(0, 6);

        Assert.AreEqual(1, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadBit_WithNegativeByteIndex_ThrowsException()
    {
        byte[] data = [0xD6];

        var reader = new BitReader(data);

        reader.ReadBit(-1, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadBit_WithByteIndexOutOfRange_ThrowsException()
    {
        byte[] data = [0xD6];

        var reader = new BitReader(data);

        reader.ReadBit(1, 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadBit_WithNegativeBitIndex_ThrowsException()
    {
        byte[] data = [0xD6];

        var reader = new BitReader(data);

        reader.ReadBit(0, -1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadBit_WithBitIndexGreaterThanSeven_ThrowsException()
    {
        byte[] data = [0xD6];

        var reader = new BitReader(data);

        reader.ReadBit(0, 8);
    }

    [TestMethod]
    public void ReadByte_ReturnsExpectedByte()
    {
        byte[] data = [0x00, 0xD6, 0x69];

        var reader = new BitReader(data, 1);

        byte result = reader.ReadByte(1);

        Assert.AreEqual(0x69, result);
    }

    [TestMethod]
    public void ReadBytes_ReturnsExpectedBytes()
    {
        byte[] data = [0x00, 0xD6, 0x69, 0xAC];

        var reader = new BitReader(data, 1);

        byte[] result = reader.ReadBytes(0, 2);

        CollectionAssert.AreEqual(new byte[] { 0xD6, 0x69 }, result);
    }

    [TestMethod]
    public void ReadBitsAsByte_SingleByte_ReturnsExpectedValue()
    {
        byte[] data = [0xD6]; // 11010110

        var reader = new BitReader(data);

        byte result = reader.ReadBitsAsByte(0, 5, 3); // bits 5..3 => 010

        Assert.AreEqual((byte)0b010, result);
    }

    [TestMethod]
    public void ReadBitsAsByte_CrossByte_ReturnsExpectedValue()
    {
        byte[] data = [0xD6, 0x69]; // 11010110 01101001

        var reader = new BitReader(data);

        byte result = reader.ReadBitsAsByte(0, 2, 5); // 110 + 01 => 11001

        Assert.AreEqual((byte)0b11001, result);
    }

    [TestMethod]
    public void ReadBitsAsByte_WithByteOffset_ReturnsExpectedValue()
    {
        byte[] data = [0x00, 0xD6, 0x69];

        var reader = new BitReader(data, 1);

        byte result = reader.ReadBitsAsByte(0, 2, 5);

        Assert.AreEqual((byte)0b11001, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadBitsAsByte_WithInvalidBitCount_ThrowsException()
    {
        byte[] data = [0xD6, 0x69];

        var reader = new BitReader(data);

        reader.ReadBitsAsByte(0, 2, 0);
    }

    [TestMethod]
    public void ReadBitsAsInt_SingleByte_ReturnsExpectedValue()
    {
        byte[] data = [0xD6]; // 11010110

        var reader = new BitReader(data);

        long result = reader.ReadBitsAsInt(0, 5, 3); // bits 5..3 => 010

        Assert.AreEqual(0b010L, result);
    }

    [TestMethod]
    public void ReadBitsAsInt_CrossBytesWithoutRemainder_ReturnsExpectedValue()
    {
        byte[] data = [0xD6, 0x69]; // 11010110 01101001

        var reader = new BitReader(data);

        long result = reader.ReadBitsAsInt(0, 3, 12); // 0110 01101001

        Assert.AreEqual(0x669L, result);
    }

    [TestMethod]
    public void ReadBitsAsInt_CrossBytesWithRemainder_ReturnsExpectedValue()
    {
        byte[] data = [0xD6, 0x69, 0xAC]; // 11010110 01101001 10101100

        var reader = new BitReader(data);

        long result = reader.ReadBitsAsInt(0, 2, 13); // 110 01101001 10

        Assert.AreEqual(0x19A6L, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ReadBitsAsInt_WithInvalidBitCount_ThrowsException()
    {
        byte[] data = [0xD6, 0x69];

        var reader = new BitReader(data);

        reader.ReadBitsAsInt(0, 2, 65);
    }
}