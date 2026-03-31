namespace Vinpay.Utils.Data
{
    /// <summary>
    /// Provides utility methods for creating bit masks for byte values.
    /// </summary>
    public class BitMaskUtil
    {
        /// <summary>
        /// Returns a byte mask with the specified number of least significant bits set to 1.
        /// </summary>
        /// <param name="bitCounts">The number of least significant bits to set, in the range 1 to 8.</param>
        /// <returns>A byte with the specified number of least significant bits set to 1.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when bitCounts is not between 1 and 8, inclusive.</exception>
        public static byte GetMultiBitMask(int bitCounts)
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

        /// <summary>
        /// Returns a bitmask with a single bit set at the specified zero-based bit index.
        /// </summary>
        /// <param name="bitIndex">The zero-based index of the bit to set, in the range 0 to 7.</param>
        /// <returns>A byte with only the specified bit set.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when bitIndex is less than 0 or greater than 7.</exception>
        public static byte GetBitMask(int bitIndex)
        {
            return bitIndex switch
            {
                0 => 0b1,
                1 => 0b10,
                2 => 0b100,
                3 => 0b1000,
                4 => 0b10000,
                5 => 0b100000,
                6 => 0b1000000,
                7 => 0b10000000,
                _ => throw new ArgumentOutOfRangeException(nameof(bitIndex), "bitIndex must be in range 0-7.")
            };
        }
    }
}
