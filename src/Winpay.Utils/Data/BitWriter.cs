namespace Winpay.Utils.Data
{
    /// <summary>
    /// Provides functionality for writing individual bits to a byte array.
    /// </summary>
    public class BitWriter
    {
        #region Properties

        /// <summary>
        /// Gets the binary data associated with the object.
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// Gets the zero-based byte offset within the underlying data source.
        /// </summary>
        public int ByteOffset { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BitWriter class using the specified byte array.
        /// </summary>
        /// <param name="data">The byte array to use for writing bits.</param>
        public BitWriter(byte[] data)
        {
            Data = data;
            ByteOffset = 0;
        }

        /// <summary>
        /// Initializes a new instance of the BitWriter class using the specified data buffer and byte offset.
        /// </summary>
        /// <param name="data">The byte array to write bits into.</param>
        /// <param name="byteOffset">The zero-based index in the data array at which to begin writing.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when byteOffset is less than zero or greater than or equal to the length of data.</exception>
        public BitWriter(byte[] data, int byteOffset)
        {
            if (byteOffset < 0 || byteOffset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(byteOffset), "byteOffset is out of range.");
            }

            Data = data;
            ByteOffset = byteOffset;
        }

        #endregion

        #region Private Methods

        private int GetActualByteIndex(int byteIndex)
        {
            int actualByteIndex = byteIndex + ByteOffset;
            if (byteIndex < 0 || actualByteIndex < 0 || actualByteIndex >= Data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(byteIndex), "byteIndex is out of range.");
            }

            return actualByteIndex;
        }

        private void EnsureByteRange(int byteIndex, int byteCount)
        {
            int actualByteIndex = byteIndex + ByteOffset;
            if (byteIndex < 0 || byteCount < 0 || actualByteIndex < 0 || actualByteIndex + byteCount > Data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(byteIndex), "byteIndex is out of range.");
            }
        }

        private void WriteBitInternal(int actualByteIndex, int bitIndex, ulong value)
        {
            var mask = BitMaskUtil.GetBitMask(bitIndex);
            if (value == 1)
            {
                Data[actualByteIndex] |= mask;
            }
            else
            {
                Data[actualByteIndex] &= (byte)~mask;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes a single bit to the specified byte and bit index.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void WriteBit(int byteIndex, int bitIndex, ulong value)
        {
            int actualByteIndex = GetActualByteIndex(byteIndex);

            if (bitIndex < 0 || bitIndex > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "bitIndex must be in range 0-7.");
            }

            if (value != 0 && value != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value must be either 0 or 1.");
            }

            WriteBitInternal(actualByteIndex, bitIndex, value);
        }

        /// <summary>
        /// Writes a single bit to the specified byte and bit index based on a Boolean value.
        /// </summary>
        /// <param name="byteIndex">The zero-based index of the target byte.</param>
        /// <param name="bitIndex">The zero-based index of the bit within the byte.</param>
        /// <param name="value">The Boolean value to write as a bit.</param>
        public void WriteBitFromBool(int byteIndex, int bitIndex, bool value)
        {
            WriteBit(byteIndex, bitIndex, value ? 1ul : 0ul);
        }

        /// <summary>
        /// Writes an integer value to the specified byte and bit index, spanning across multiple bits if necessary.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="bitIndex"></param>
        /// <param name="bitCount"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void WriteInt(int byteIndex, int bitIndex, int bitCount, ulong value)
        {
            if (bitIndex < 0 || bitIndex > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(bitIndex), "bitIndex must be in range 0-7.");
            }

            if (bitCount < 1 || bitCount > 64)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount), "bitCounts must be in range 1-64.");
            }

            if (bitCount < 64 && value >= (1ul << bitCount))
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"value is too large to fit in {bitCount} bits.");
            }

            int firstBitCount = bitIndex + 1;
            int remainingBitCount = Math.Max(0, bitCount - firstBitCount);
            int requiredByteCount = 1 + (remainingBitCount + 7) / 8;

            EnsureByteRange(byteIndex, requiredByteCount);

            int actualByteIndex = byteIndex + ByteOffset;
            int maxBitCountInFirstByte = bitIndex + 1;
            if (maxBitCountInFirstByte >= bitCount)
            {
                int leftShift = maxBitCountInFirstByte - bitCount;
                var mask = (byte)~(BitMaskUtil.GetMultiBitMask(bitCount) << leftShift);
                Data[actualByteIndex] = (byte)((ulong)(mask & Data[actualByteIndex]) + (value << leftShift));
                return;
            }

            int lastBitCount = bitCount - firstBitCount;
            if (lastBitCount <= 8)
            {
                var firstMask = ~BitMaskUtil.GetMultiBitMask(firstBitCount);
                Data[actualByteIndex] = (byte)((ulong)(firstMask & Data[actualByteIndex]) + (value >> lastBitCount));

                var secondMask = (byte)~(BitMaskUtil.GetMultiBitMask(lastBitCount) << 8 - lastBitCount);
                Data[actualByteIndex + 1] = (byte)((ulong)(secondMask & Data[actualByteIndex + 1]) + ((value & BitMaskUtil.GetMultiBitMask(lastBitCount)) << 8 - lastBitCount));
                return;
            }

            int firstMask1 = ~BitMaskUtil.GetMultiBitMask(firstBitCount);
            Data[actualByteIndex] = (byte)((ulong)(firstMask1 & Data[actualByteIndex]) + (value >> bitCount - firstBitCount));

            int fullByteCount = (bitCount - firstBitCount) / 8;
            for (int i = 0; i < fullByteCount; i++)
            {
                int rightShift = bitCount - firstBitCount - (i + 1) * 8;
                Data[actualByteIndex + 1 + i] = (byte)(value >> rightShift & 255);
            }

            lastBitCount = bitCount - firstBitCount - fullByteCount * 8;
            if (lastBitCount > 0)
            {
                var lastMask = (byte)~(BitMaskUtil.GetMultiBitMask(lastBitCount) << 8 - lastBitCount);
                Data[actualByteIndex + fullByteCount + 1] = (byte)((ulong)(lastMask & Data[actualByteIndex + fullByteCount + 1]) + ((value & BitMaskUtil.GetMultiBitMask(lastBitCount)) << 8 - lastBitCount));
            }
        }

        /// <summary>
        /// Writes a byte value at the specified index in the data array.
        /// </summary>
        /// <param name="byteIndex">The zero-based index at which to write the byte.</param>
        /// <param name="value">The byte value to write.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when byteIndex is less than zero or greater than or equal to the length of the data array.</exception>
        public void WriteByte(int byteIndex, byte value)
        {
            int actualByteIndex = GetActualByteIndex(byteIndex);

            Data[actualByteIndex] = value;
        }

        /// <summary>
        /// Writes a byte array starting at the specified index in the data array.
        /// </summary>
        /// <param name="byteIndex"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void WriteBytes(int byteIndex, byte[] value)
        {
            EnsureByteRange(byteIndex, value.Length);

            Array.Copy(value, 0, Data, byteIndex + ByteOffset, value.Length);
        }

        #endregion
    }
}
