namespace Vinpay.Utils.Maths
{
    /// <summary>
    /// A tool for converting angles and radians to each other
    /// </summary>
    public static class AngleConverter
    {
        private const double TO_DEGREE_FACTOR = 57.29577951308232088;
        private const double TO_RADIAN_FACTOR = 0.017453292519943296;

        /// <summary>
        /// Convert radian to degree.
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double RadianToDegree(this double radian)
        {
            return radian * TO_DEGREE_FACTOR;
        }

        /// <summary>
        /// Convert degree to radian.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double DegreeToRadian(this double degree)
        {
            return degree * TO_RADIAN_FACTOR;
        }
    }
}
