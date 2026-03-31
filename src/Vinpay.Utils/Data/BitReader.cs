namespace Vinpay.Utils.Data
{
    /// <summary>
    /// Bit Reader
    /// </summary>
    public class BitReader
    {
        #region Private Methods

        private static byte GetBitMask(int bitCounts)
        {
            return bitCounts switch
            {
                1 => 0b1,
                2 => 0b11,
                3 => 0b111,
                4 => 0b1111,
                5 => 0b11111,
                6 => 0b111111,
                7 => 0b1111111,
                8 => 0b11111111,
                _ => throw new ArgumentOutOfRangeException(nameof(bitCounts), "bitCounts must be in range 1-8.")
            };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Data
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// Gets the zero-based byte offset within the underlying data source.
        /// </summary>
        public int ByteOffset { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public BitReader(byte[] data)
        {
            Data = data;
            ByteOffset = 0;
        }

        /// <summary>
        /// Initializes a new instance of the BitReader class starting at the specified byte offset in the provided data array.
        /// </summary>
        /// <param name="data">The byte array containing the data to read.</param>
        /// <param name="byteOffset">The zero-based index in the data array at which to begin reading.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when byteOffset is less than zero or greater than or equal to the length of data.</exception>
        public BitReader(byte[] data, int byteOffset)
        {
            if (byteOffset < 0 || byteOffset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(byteOffset), "byteOffset is out of range.");
            }

            Data = data;
            ByteOffset = byteOffset;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Read a bit and convert to boolean
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        public bool ReadBitAsBool(int byteIndex, int bitIndex)
        {
            int result = ReadBit(byteIndex, bitIndex);
            return result == 1;
        }

        /// <summary>
        /// Read a bit
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int ReadBit(int byteIndex, int bitIndex)
        {
            if (byteIndex < 0 || byteIndex + ByteOffset >= Data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(byteIndex), "bytesOffset is out of range.");
            }

            if (bitIndex < 0 || bitIndex > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "bitIndex must be in range 0-7.");
            }

            return Data[byteIndex + ByteOffset] >> bitIndex & 1;
        }

        /// <summary>
        /// Read a byte
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <returns></returns>
        public byte ReadByte(int byteIndex)
        {
            return Data[byteIndex + ByteOffset];
        }

        /// <summary>
        /// Read multiple bytes
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="bytesCount"></param>
        /// <returns></returns>
        public byte[] ReadBytes(int byteIndex, int bytesCount)
        {
            var newBytes = new byte[bytesCount];
            Array.Copy(Data, byteIndex + ByteOffset, newBytes, 0, bytesCount);
            return newBytes;
        }

        /// <summary>
        /// Read integer with bit offset and bit counts
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="bitIndex"></param>
        /// <param name="bitCount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public long ReadBitsAsInt(int byteIndex, int bitIndex, int bitCount)
        {
            if (byteIndex < 0 || byteIndex >= Data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(byteIndex), "bytesOffset is out of range.");
            }

            if (bitIndex > 7 || bitIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "bitIndex must be in range 0-7.");
            }

            if (bitCount > 64 || bitCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount), "bitCount must be in range 1-64");
            }

            // Single byte case
            if (bitIndex + 1 - bitCount >= 0)
            {
                int rightShift = bitIndex + 1 - bitCount;
                byte mask = GetBitMask(bitCount);
                long onlyFirstByteValue = Data[byteIndex + ByteOffset] >> rightShift & mask;
                return onlyFirstByteValue;
            }

            int firstByteBitCount = bitIndex + 1;
            int middleByteCount = (bitCount - firstByteBitCount) / 8;
            int lastByteBitCount = (bitCount - firstByteBitCount) % 8;

            byte firstByteMask = GetBitMask(firstByteBitCount);
            int firstByteValue = (byte)(Data[byteIndex + ByteOffset] & firstByteMask);

            int result = firstByteValue << bitCount - firstByteBitCount;
            for (int i = 0; i < middleByteCount; i++)
            {
                result += Data[byteIndex + ByteOffset + 1 + i] << lastByteBitCount + (middleByteCount - 1 - i) * 8;
            }

            if (lastByteBitCount > 0)
            {
                int lastByteValue = Data[byteIndex + ByteOffset + 1 + middleByteCount] >> 8 - lastByteBitCount;
                result += lastByteValue;
            }

            return result;
        }

        /// <summary>
        /// Reads a specified number of bits from the data buffer starting at the given byte and bit index, and returns them as a byte.
        /// </summary>
        /// <param name="byteIndex">The zero-based index of the byte in the data buffer to start reading from.</param>
        /// <param name="bitIndex">The zero-based index of the bit within the starting byte to begin reading.</param>
        /// <param name="bitCount">The number of bits to read, in the range 1 to 8.</param>
        /// <returns>A byte containing the extracted bits, right-aligned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when byteIndex, bitIndex, or bitCount is outside the valid range.</exception>
        public byte ReadBitsAsByte(int byteIndex, int bitIndex, int bitCount)
        {
            if (byteIndex < 0 || byteIndex >= Data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(byteIndex), "bytesOffset is out of range.");
            }

            if (bitIndex > 7 || bitIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "bitIndex must be in range 0-7.");
            }

            if (bitCount > 8 || bitCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount), "bitCount must be in range 1-8");
            }

            // Single byte case
            if (bitIndex + 1 - bitCount >= 0)
            {
                int rightShift = bitIndex + 1 - bitCount;
                byte mask = GetBitMask(bitCount);
                byte onlyFirstByteValue = (byte)(Data[byteIndex + ByteOffset] >> rightShift & mask);
                return onlyFirstByteValue;
            }

            int firstByteBitCount = bitIndex + 1;
            int lastByteBitCount = bitCount - firstByteBitCount;

            byte firstByteMask = GetBitMask(firstByteBitCount);
            int firstByteValue = (byte)(Data[byteIndex + ByteOffset] & firstByteMask);

            int lastByteValue = Data[byteIndex + ByteOffset + 1] >> 8 - lastByteBitCount;
            return (byte)((firstByteValue << lastByteBitCount) + lastByteValue);
        }

        #endregion
    }
}
