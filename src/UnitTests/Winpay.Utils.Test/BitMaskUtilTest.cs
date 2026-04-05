using Winpay.Utils.Data;

namespace Winpay.Utils.Test;

[TestClass]
public class BitMaskUtilTest
{
    [TestMethod]
    [DataRow(1, 0b00000001)]
    [DataRow(2, 0b00000011)]
    [DataRow(3, 0b00000111)]
    [DataRow(4, 0b00001111)]
    [DataRow(5, 0b00011111)]
    [DataRow(6, 0b00111111)]
    [DataRow(7, 0b01111111)]
    [DataRow(8, 0b11111111)]
    public void GetMultiBitMask_ReturnsExpectedMask(int bitCounts, int expected)
    {
        byte result = BitMaskUtil.GetMultiBitMask(bitCounts);

        Assert.AreEqual((byte)expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetMultiBitMask_WithZero_ThrowsException()
    {
        BitMaskUtil.GetMultiBitMask(0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetMultiBitMask_WithNegativeValue_ThrowsException()
    {
        BitMaskUtil.GetMultiBitMask(-1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetMultiBitMask_WithValueGreaterThanEight_ThrowsException()
    {
        BitMaskUtil.GetMultiBitMask(9);
    }

    [TestMethod]
    [DataRow(0, 0b00000001)]
    [DataRow(1, 0b00000010)]
    [DataRow(2, 0b00000100)]
    [DataRow(3, 0b00001000)]
    [DataRow(4, 0b00010000)]
    [DataRow(5, 0b00100000)]
    [DataRow(6, 0b01000000)]
    [DataRow(7, 0b10000000)]
    public void GetBitMask_ReturnsExpectedMask(int bitIndex, int expected)
    {
        byte result = BitMaskUtil.GetBitMask(bitIndex);

        Assert.AreEqual((byte)expected, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetBitMask_WithNegativeValue_ThrowsException()
    {
        BitMaskUtil.GetBitMask(-1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetBitMask_WithValueGreaterThanSeven_ThrowsException()
    {
        BitMaskUtil.GetBitMask(8);
    }
}