using Vinpay.Utils.Maths;

namespace Vinpay.Utils.Test;

/// <summary>
/// Unit tests for AngleConverter class
/// </summary>
[TestClass]
public class AngleConverterTest
{
    #region DegreeToRadian Tests

    /// <summary>
    /// Test conversion from degree to radian
    /// </summary>
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(30)]
    [DataRow(45)]
    [DataRow(60)]
    [DataRow(72)]
    [DataRow(90)]
    [DataRow(180)]
    [DataRow(270)]
    [DataRow(360)]
    [DataRow(925)]
    public void DegreeToRadianTest(double degree)
    {
        double radian = degree.DegreeToRadian();
        double expected = degree * Math.PI / 180.0;
        Assert.AreEqual(expected, radian, 1E-8, $"Conversion failed for {degree} degrees");
    }

    #endregion

    #region RadianToDegree Tests

    /// <summary>
    /// Test conversion from radian to degree
    /// </summary>
    [TestMethod]
    [DataRow(0)]
    [DataRow(Math.PI / 6)]
    [DataRow(Math.PI / 4)]
    [DataRow(Math.PI / 3)]
    [DataRow(Math.PI / 2)]
    [DataRow(Math.PI)]
    [DataRow(Math.PI * 1.5)]
    [DataRow(Math.PI * 2)]
    [DataRow(1)]
    [DataRow(3)]
    [DataRow(12.35)]
    public void RadianToDegreeTest(double radian)
    {
        double degree = radian.RadianToDegree();
        double expected = radian * 180.0 / Math.PI;
        Assert.AreEqual(expected, degree, 1E-8, $"Conversion failed for {radian} radians");
    }

    #endregion

    #region Edge Cases and Boundary Tests

    /// <summary>
    /// Test degree to radian with negative values
    /// </summary>
    [TestMethod]
    [DataRow(-90)]
    [DataRow(-180)]
    [DataRow(-360)]
    [DataRow(-1)]
    public void DegreeToRadian_NegativeValues_ReturnsCorrectResult(double degree)
    {
        double radian = degree.DegreeToRadian();
        double expected = degree * Math.PI / 180.0;
        Assert.AreEqual(expected, radian, 1E-8, $"Conversion failed for {degree} degrees");
    }

    /// <summary>
    /// Test radian to degree with negative values
    /// </summary>
    [TestMethod]
    [DataRow(-Math.PI / 2)]
    [DataRow(-Math.PI)]
    [DataRow(-1)]
    public void RadianToDegree_NegativeValues_ReturnsCorrectResult(double radian)
    {
        double degree = radian.RadianToDegree();
        double expected = radian * 180.0 / Math.PI;
        Assert.AreEqual(expected, degree, 1E-8, $"Conversion failed for {radian} radians");
    }

    /// <summary>
    /// Test degree to radian with decimal values
    /// </summary>
    [TestMethod]
    [DataRow(0.5)]
    [DataRow(15.75)]
    [DataRow(45.5)]
    [DataRow(180.25)]
    public void DegreeToRadian_DecimalValues_ReturnsCorrectResult(double degree)
    {
        double radian = degree.DegreeToRadian();
        double expected = degree * Math.PI / 180.0;
        Assert.AreEqual(expected, radian, 1E-8, $"Conversion failed for {degree} degrees");
    }

    /// <summary>
    /// Test radian to degree with decimal values
    /// </summary>
    [TestMethod]
    [DataRow(0.5)]
    [DataRow(1.5)]
    [DataRow(3.14)]
    [DataRow(6.28)]
    public void RadianToDegree_DecimalValues_ReturnsCorrectResult(double radian)
    {
        double degree = radian.RadianToDegree();
        double expected = radian * 180.0 / Math.PI;
        Assert.AreEqual(expected, degree, 1E-8, $"Conversion failed for {radian} radians");
    }

    /// <summary>
    /// Test very large degree values
    /// </summary>
    [TestMethod]
    public void DegreeToRadian_LargeValue_HandlesCorrectly()
    {
        const double largeDegree = 10000;
        double radian = largeDegree.DegreeToRadian();
        double expected = largeDegree * Math.PI / 180.0;
        Assert.AreEqual(expected, radian, 1E-8, $"Conversion failed for large value {largeDegree} degrees");
    }

    /// <summary>
    /// Test very large radian values
    /// </summary>
    [TestMethod]
    public void RadianToDegree_LargeValue_HandlesCorrectly()
    {
        const double largeRadian = 1000;
        double degree = largeRadian.RadianToDegree();
        double expected = largeRadian * 180.0 / Math.PI;
        Assert.AreEqual(expected, degree, 1E-8, $"Conversion failed for large value {largeRadian} radians");
    }

    #endregion

    #region Round-trip Conversion Tests

    /// <summary>
    /// Test round-trip conversion: degree -> radian -> degree
    /// </summary>
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(30)]
    [DataRow(45)]
    [DataRow(90)]
    [DataRow(180)]
    [DataRow(360)]
    [DataRow(123.456)]
    [DataRow(-90)]
    [DataRow(-180)]
    public void RoundTrip_DegreeRadianDegree_ReturnsOriginalValue(double originalDegree)
    {
        double radian = originalDegree.DegreeToRadian();
        double backToDegree = radian.RadianToDegree();
        Assert.AreEqual(originalDegree, backToDegree, 1E-8, $"Round-trip conversion failed for {originalDegree} degrees");
    }

    /// <summary>
    /// Test round-trip conversion: radian -> degree -> radian
    /// </summary>
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(Math.PI / 6)]
    [DataRow(Math.PI / 4)]
    [DataRow(Math.PI / 2)]
    [DataRow(Math.PI)]
    [DataRow(2.5)]
    [DataRow(-1.5)]
    public void RoundTrip_RadianDegreeRadian_ReturnsOriginalValue(double originalRadian)
    {
        double degree = originalRadian.RadianToDegree();
        double backToRadian = degree.DegreeToRadian();
        Assert.AreEqual(originalRadian, backToRadian, 1E-8, $"Round-trip conversion failed for {originalRadian} radians");
    }

    #endregion

    #region Common Angle Values Tests

    /// <summary>
    /// Test common degree values to radian conversion
    /// </summary>
    [TestMethod]
    public void DegreeToRadian_CommonAngles_ReturnsCorrectResults()
    {
        // Common trigonometric angles
        Dictionary<double, double> commonAngles = new()
        {
            { 0, 0 },
            { 30, Math.PI / 6 },
            { 45, Math.PI / 4 },
            { 60, Math.PI / 3 },
            { 90, Math.PI / 2 },
            { 120, Math.PI * 2 / 3 },
            { 135, Math.PI * 3 / 4 },
            { 150, Math.PI * 5 / 6 },
            { 180, Math.PI },
            { 270, Math.PI * 3 / 2 },
            { 360, Math.PI * 2 }
        };

        foreach (var angle in commonAngles)
        {
            double radian = angle.Key.DegreeToRadian();
            Assert.AreEqual(angle.Value, radian, 1E-8, $"Failed for {angle.Key} degrees");
        }
    }

    /// <summary>
    /// Test common radian values to degree conversion
    /// </summary>
    [TestMethod]
    public void RadianToDegree_CommonAngles_ReturnsCorrectResults()
    {
        // Common trigonometric angles
        Dictionary<double, double> commonAngles = new()
        {
            { 0, 0 },
            { Math.PI / 6, 30 },
            { Math.PI / 4, 45 },
            { Math.PI / 3, 60 },
            { Math.PI / 2, 90 },
            { Math.PI * 2 / 3, 120 },
            { Math.PI * 3 / 4, 135 },
            { Math.PI * 5 / 6, 150 },
            { Math.PI, 180 },
            { Math.PI * 3 / 2, 270 },
            { Math.PI * 2, 360 }
        };

        foreach (var angle in commonAngles)
        {
            double degree = angle.Key.RadianToDegree();
            Assert.AreEqual(angle.Value, degree, 1E-8, $"Failed for {angle.Key} radians");
        }
    }

    #endregion

    #region Precision Tests

    /// <summary>
    /// Test precision with very small values
    /// </summary>
    [TestMethod]
    public void DegreeToRadian_SmallValues_PrecisionTest()
    {
        const double smallDegree = 0.001;
        double radian = smallDegree.DegreeToRadian();
        double expected = smallDegree * Math.PI / 180.0;
        Assert.AreEqual(expected, radian, 1E-15, $"Precision test failed for {smallDegree} degrees");
    }

    /// <summary>
    /// Test precision with very small radian values
    /// </summary>
    [TestMethod]
    public void RadianToDegree_SmallValues_PrecisionTest()
    {
        const double smallRadian = 0.001;
        double degree = smallRadian.RadianToDegree();
        double expected = smallRadian * 180.0 / Math.PI;
        Assert.AreEqual(expected, degree, 1E-15, $"Precision test failed for {smallRadian} radians");
    }

    #endregion
}
