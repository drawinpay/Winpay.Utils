using Winpay.Utils.Data;

namespace Winpay.Utils.Test;

[TestClass]
public class BitWriterTest
{
    [TestMethod]
    public void Constructor_WithoutOffset_SetsDataAndDefaultOffset()
    {
        byte[] data = [0x00, 0x01];

        var writer = new BitWriter(data);

        Assert.AreSame(data, writer.Data);
        Assert.AreEqual(0, writer.ByteOffset);
    }

    [TestMethod]
    public void Constructor_WithOffset_SetsDataAndOffset()
    {
        byte[] data = [0x00, 0x01, 0x02];

        var writer = new BitWriter(data, 1);

        Assert.AreSame(data, writer.Data);
        Assert.AreEqual(1, writer.ByteOffset);
    }

    [TestMethod]
    public void Constructor_WithNegativeOffset_ThrowsException()
    {
        byte[] data = [0x00, 0x01];

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new BitWriter(data, -1));
    }

    [TestMethod]
    public void Constructor_WithOffsetEqualToLength_ThrowsException()
    {
        byte[] data = [0x00, 0x01];

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => new BitWriter(data, data.Length));
    }

    [TestMethod]
    public void WriteBit_WritesOneToExpectedPosition()
    {
        byte[] data = [0x00];
        var writer = new BitWriter(data);

        writer.WriteBit(0, 6, 1);

        CollectionAssert.AreEqual(new byte[] { 0x40 }, data);
    }

    [TestMethod]
    public void WriteBit_WritesZeroToExpectedPosition()
    {
        byte[] data = [0xFF];
        var writer = new BitWriter(data);

        writer.WriteBit(0, 6, 0);

        CollectionAssert.AreEqual(new byte[] { 0xBF }, data);
    }

    [TestMethod]
    public void WriteBit_WithNegativeByteIndex_ThrowsException()
    {
        byte[] data = [0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBit(-1, 0, 1));
    }

    [TestMethod]
    public void WriteBit_WithByteIndexOutOfRange_ThrowsException()
    {
        byte[] data = [0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBit(1, 0, 1));
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(8)]
    public void WriteBit_WithInvalidBitIndex_ThrowsException(int bitIndex)
    {
        byte[] data = [0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBit(0, bitIndex, 1));
    }

    [TestMethod]
    [DataRow(2ul)]
    [DataRow(99ul)]
    public void WriteBit_WithInvalidValue_ThrowsException(ulong value)
    {
        byte[] data = [0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBit(0, 0, value));
    }

    [TestMethod]
    public void WriteBitFromBool_True_WritesOne()
    {
        byte[] data = [0x00];
        var writer = new BitWriter(data);

        writer.WriteBitFromBool(0, 0, true);

        CollectionAssert.AreEqual(new byte[] { 0x01 }, data);
    }

    [TestMethod]
    public void WriteBitFromBool_False_WritesZero()
    {
        byte[] data = [0xFF];
        var writer = new BitWriter(data);

        writer.WriteBitFromBool(0, 0, false);

        CollectionAssert.AreEqual(new byte[] { 0xFE }, data);
    }

    [TestMethod]
    public void WriteInt_SingleByte_WritesExpectedBitsAndPreservesOtherBits()
    {
        byte[] data = [0xFF];
        var writer = new BitWriter(data);

        writer.WriteInt(0, 5, 3, 0b010);

        CollectionAssert.AreEqual(new byte[] { 0xD7 }, data);
    }

    [TestMethod]
    public void WriteInt_TwoBytes_WritesExpectedBits()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        writer.WriteInt(0, 2, 5, 0b11001);

        CollectionAssert.AreEqual(new byte[] { 0x06, 0x40 }, data);
    }

    [TestMethod]
    public void WriteInt_MultipleBytesWithTrailingBits_WritesExpectedBits()
    {
        byte[] data = [0x00, 0x00, 0x00];
        var writer = new BitWriter(data);

        writer.WriteInt(0, 2, 13, 0x19A6);

        CollectionAssert.AreEqual(new byte[] { 0x06, 0x69, 0x80 }, data);
    }

    [TestMethod]
    public void WriteInt_MultipleBytesWithoutTrailingBits_WritesExpectedBits()
    {
        byte[] data = [0xF0, 0x00, 0x00];
        var writer = new BitWriter(data);

        writer.WriteInt(0, 3, 20, 0xABCDE);

        CollectionAssert.AreEqual(new byte[] { 0xFA, 0xBC, 0xDE }, data);
    }

    [TestMethod]
    public void WriteInt_SixtyFourBits_AllowsFullUlongRange()
    {
        byte[] data = new byte[8];
        var writer = new BitWriter(data);

        writer.WriteInt(0, 7, 64, ulong.MaxValue);

        CollectionAssert.AreEqual(new byte[]
        {
            0xFF, 0xFF, 0xFF, 0xFF,
            0xFF, 0xFF, 0xFF, 0xFF
        }, data);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(8)]
    public void WriteInt_WithInvalidBitIndex_ThrowsException(int bitIndex)
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteInt(0, bitIndex, 1, 0));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(65)]
    public void WriteInt_WithInvalidBitCount_ThrowsException(int bitCount)
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteInt(0, 0, bitCount, 0));
    }

    [TestMethod]
    public void WriteInt_WithValueTooLarge_ThrowsException()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteInt(0, 2, 5, 0b100000));
    }

    [TestMethod]
    public void WriteInt_WithNegativeByteIndex_ThrowsException()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteInt(-1, 2, 5, 0));
    }

    [TestMethod]
    public void WriteInt_WithInsufficientByteRange_ThrowsException()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteInt(1, 2, 5, 0));
    }

    [TestMethod]
    public void WriteByte_WritesExpectedByte()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        writer.WriteByte(1, 0x69);

        CollectionAssert.AreEqual(new byte[] { 0x00, 0x69 }, data);
    }

    [TestMethod]
    public void WriteByte_WithNegativeByteIndex_ThrowsException()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteByte(-1, 0x69));
    }

    [TestMethod]
    public void WriteByte_WithByteIndexOutOfRange_ThrowsException()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteByte(2, 0x69));
    }

    [TestMethod]
    public void WriteBytes_WritesExpectedRange()
    {
        byte[] data = [0x00, 0x00, 0x00, 0x00];
        var writer = new BitWriter(data);

        writer.WriteBytes(1, [0xD6, 0x69]);

        CollectionAssert.AreEqual(new byte[] { 0x00, 0xD6, 0x69, 0x00 }, data);
    }

    [TestMethod]
    public void WriteBytes_WithNegativeByteIndex_ThrowsException()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBytes(-1, [0xD6]));
    }

    [TestMethod]
    public void WriteBytes_WithInsufficientByteRange_ThrowsException()
    {
        byte[] data = [0x00, 0x00];
        var writer = new BitWriter(data);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBytes(1, [0xD6, 0x69]));
    }

    [TestMethod]
    public void WriteBit_WithByteOffset_WritesToOffsetByte()
    {
        byte[] data = [0xAA, 0x00, 0x55];

        var writer = new BitWriter(data, 1);

        writer.WriteBit(0, 6, 1);

        CollectionAssert.AreEqual(new byte[] { 0xAA, 0x40, 0x55 }, data);
    }

    [TestMethod]
    public void WriteBit_WithByteOffsetAndOutOfRange_ThrowsException()
    {
        byte[] data = [0xAA, 0x00];

        var writer = new BitWriter(data, 1);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBit(1, 0, 1));
    }

    [TestMethod]
    public void WriteBitFromBool_WithByteOffset_WritesToOffsetByte()
    {
        byte[] data = [0xAA, 0xFF, 0x55];

        var writer = new BitWriter(data, 1);

        writer.WriteBitFromBool(0, 1, false);

        CollectionAssert.AreEqual(new byte[] { 0xAA, 0xFD, 0x55 }, data);
    }

    [TestMethod]
    public void WriteInt_WithByteOffset_WritesToOffsetBytes()
    {
        byte[] data = [0xAA, 0x00, 0x00, 0x00, 0x55];

        var writer = new BitWriter(data, 1);

        writer.WriteInt(0, 2, 13, 0x19A6);

        CollectionAssert.AreEqual(new byte[] { 0xAA, 0x06, 0x69, 0x80, 0x55 }, data);
    }

    [TestMethod]
    public void WriteInt_WithByteOffsetAndInsufficientByteRange_ThrowsException()
    {
        byte[] data = [0xAA, 0x00, 0x00];

        var writer = new BitWriter(data, 1);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteInt(1, 2, 13, 0x19A6));
    }

    [TestMethod]
    public void WriteByte_WithByteOffset_WritesToOffsetByte()
    {
        byte[] data = [0xAA, 0x00, 0x55];

        var writer = new BitWriter(data, 1);

        writer.WriteByte(0, 0x69);

        CollectionAssert.AreEqual(new byte[] { 0xAA, 0x69, 0x55 }, data);
    }

    [TestMethod]
    public void WriteByte_WithByteOffsetAndOutOfRange_ThrowsException()
    {
        byte[] data = [0x00, 0x00];

        var writer = new BitWriter(data, 1);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteByte(1, 0x69));
    }

    [TestMethod]
    public void WriteBytes_WithByteOffset_WritesToOffsetRange()
    {
        byte[] data = [0xAA, 0x00, 0x00, 0x55];

        var writer = new BitWriter(data, 1);

        writer.WriteBytes(0, [0xD6, 0x69]);

        CollectionAssert.AreEqual(new byte[] { 0xAA, 0xD6, 0x69, 0x55 }, data);
    }

    [TestMethod]
    public void WriteBytes_WithByteOffsetAndOutOfRange_ThrowsException()
    {
        byte[] data = [0x00, 0x00, 0x00];

        var writer = new BitWriter(data, 1);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => writer.WriteBytes(1, [0xD6, 0x69]));
    }
}
