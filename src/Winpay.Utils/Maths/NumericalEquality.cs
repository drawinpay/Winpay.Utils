namespace Winpay.Utils.Maths
{
    /// <summary>
    /// Provides extension methods for comparing floating-point numbers with a specified precision (epsilon).
    /// </summary>
    public static class NumericalEquality    
    {
        /// <summary>
        /// Determine whether two double-type values are equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Equals(this double left, double right, double epsilon = 1e-10)
        {
            return Math.Abs(left - right) < epsilon;
        }

        /// <summary>
        /// Determine whether two float-type values are equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool Equals(this float left, float right, float epsilon = 1e-6f)
        {
            return Math.Abs(left - right) < epsilon;
        }

        /// <summary>
        /// Determine whether two double-type values are approximately equal
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static bool ApproximatelyEquals(this double left, double right, double epsilon = 1e-10)
        {
            return Math.Abs(left - right) < epsilon;
        }
    }
}
