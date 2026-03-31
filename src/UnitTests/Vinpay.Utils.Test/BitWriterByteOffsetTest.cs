using Vinpay.Utils.Data;

namespace Vinpay.Utils.Test;

[TestClass]
public class BitWriterByteOffsetTest
{
    [TestMethod]
    public void WriteBit_WithByteOffset_WritesToOffsetByte()
    {
        byte[] data = [0xAA, 0x00, 0x55];

        var writer = new BitWriter(data, 1);

        writer.WriteBit(0, 6, 1);

        CollectionAssert.AreEqual(new byte[] { 0xAA, 0x40, 0x55 }, data);
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
    public void WriteByte_WithByteOffset_WritesToOffsetByte()
    {
        byte[] data = [0xAA, 0x00, 0x55];

        var writer = new BitWriter(data, 1);

        writer.WriteByte(0, 0x69);

        CollectionAssert.AreEqual(new byte[] { 0xAA, 0x69, 0x55 }, data);
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
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void WriteByte_WithByteOffsetAndOutOfRange_ThrowsException()
    {
        byte[] data = [0x00, 0x00];

        var writer = new BitWriter(data, 1);

        writer.WriteByte(1, 0x69);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void WriteBytes_WithByteOffsetAndOutOfRange_ThrowsException()
    {
        byte[] data = [0x00, 0x00, 0x00];

        var writer = new BitWriter(data, 1);

        writer.WriteBytes(1, [0xD6, 0x69]);
    }
}