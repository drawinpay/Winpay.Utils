using Vinpay.Utils.Maths;

namespace Vinpay.Utils.Test;

[TestClass]
public class NumericalEqualityTest
{
    [TestMethod]
    [DataRow(1d, 1d, true)]
    [DataRow(1d, 1.00000000005d, true)]
    [DataRow(1d, 1.0000000001d, false)]
    [DataRow(1d, 1.0000000002d, false)]
    [DataRow(-1d, -1.00000000005d, true)]
    [DataRow(0d, -0d, true)]
    public void DoubleEquals_WithDefaultEpsilon_ReturnsExpectedResult(double left, double right, bool expected)
    {
        bool result = NumericalEquality.Equals(left, right);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow(1f, 1f, true)]
    [DataRow(1f, 1.0000005f, true)]
    [DataRow(1f, 1.0000011f, false)]
    [DataRow(1f, 1.000002f, false)]
    [DataRow(-1f, -1.0000005f, true)]
    [DataRow(0f, -0f, true)]
    public void FloatEquals_WithDefaultEpsilon_ReturnsExpectedResult(float left, float right, bool expected)
    {
        bool result = NumericalEquality.Equals(left, right);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow(100d, 100.4d, 0.5d, true)]
    [DataRow(100d, 100.5d, 0.5d, false)]
    [DataRow(100d, 100.6d, 0.5d, false)]
    public void DoubleEquals_WithCustomEpsilon_ReturnsExpectedResult(double left, double right, double epsilon, bool expected)
    {
        bool result = NumericalEquality.Equals(left, right, epsilon);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow(100f, 100.4f, 0.5f, true)]
    [DataRow(100f, 100.5f, 0.5f, false)]
    [DataRow(100f, 100.6f, 0.5f, false)]
    public void FloatEquals_WithCustomEpsilon_ReturnsExpectedResult(float left, float right, float epsilon, bool expected)
    {
        bool result = NumericalEquality.Equals(left, right, epsilon);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void DoubleEquals_WithZeroOrNegativeEpsilon_ReturnsFalse()
    {
        Assert.IsFalse(NumericalEquality.Equals(1d, 1d, 0d));
        Assert.IsFalse(NumericalEquality.Equals(1d, 1d, -1d));
    }

    [TestMethod]
    public void FloatEquals_WithZeroOrNegativeEpsilon_ReturnsFalse()
    {
        Assert.IsFalse(NumericalEquality.Equals(1f, 1f, 0f));
        Assert.IsFalse(NumericalEquality.Equals(1f, 1f, -1f));
    }

    [TestMethod]
    public void DoubleEquals_WithSpecialValues_ReturnsFalse()
    {
        Assert.IsFalse(NumericalEquality.Equals(double.NaN, 1d));
        Assert.IsFalse(NumericalEquality.Equals(1d, double.NaN));
        Assert.IsFalse(NumericalEquality.Equals(double.NaN, double.NaN));
        Assert.IsFalse(NumericalEquality.Equals(double.PositiveInfinity, double.PositiveInfinity));
        Assert.IsFalse(NumericalEquality.Equals(double.NegativeInfinity, double.NegativeInfinity));
        Assert.IsFalse(NumericalEquality.Equals(double.PositiveInfinity, double.NegativeInfinity));
    }

    [TestMethod]
    public void FloatEquals_WithSpecialValues_ReturnsFalse()
    {
        Assert.IsFalse(NumericalEquality.Equals(float.NaN, 1f));
        Assert.IsFalse(NumericalEquality.Equals(1f, float.NaN));
        Assert.IsFalse(NumericalEquality.Equals(float.NaN, float.NaN));
        Assert.IsFalse(NumericalEquality.Equals(float.PositiveInfinity, float.PositiveInfinity));
        Assert.IsFalse(NumericalEquality.Equals(float.NegativeInfinity, float.NegativeInfinity));
        Assert.IsFalse(NumericalEquality.Equals(float.PositiveInfinity, float.NegativeInfinity));
    }

    [TestMethod]
    public void DoubleEquals_WithIdenticalBoundaryValues_ReturnsTrue()
    {
        Assert.IsTrue(NumericalEquality.Equals(double.MaxValue, double.MaxValue));
        Assert.IsTrue(NumericalEquality.Equals(double.MinValue, double.MinValue));
        Assert.IsTrue(NumericalEquality.Equals(double.Epsilon, double.Epsilon));
    }

    [TestMethod]
    public void FloatEquals_WithIdenticalBoundaryValues_ReturnsTrue()
    {
        Assert.IsTrue(NumericalEquality.Equals(float.MaxValue, float.MaxValue));
        Assert.IsTrue(NumericalEquality.Equals(float.MinValue, float.MinValue));
        Assert.IsTrue(NumericalEquality.Equals(float.Epsilon, float.Epsilon));
    }

    [TestMethod]
    [DataRow(1d, 1d, true)]
    [DataRow(1d, 1.00000000005d, true)]
    [DataRow(1d, 1.0000000001d, false)]
    [DataRow(1d, 1.0000000002d, false)]
    [DataRow(-1d, -1.00000000005d, true)]
    [DataRow(0d, -0d, true)]
    public void ApproximatelyEquals_WithDefaultEpsilon_ReturnsExpectedResult(double left, double right, bool expected)
    {
        bool result = NumericalEquality.ApproximatelyEquals(left, right);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow(100d, 100.4d, 0.5d, true)]
    [DataRow(100d, 100.5d, 0.5d, false)]
    [DataRow(100d, 100.6d, 0.5d, false)]
    public void ApproximatelyEquals_WithCustomEpsilon_ReturnsExpectedResult(double left, double right, double epsilon, bool expected)
    {
        bool result = NumericalEquality.ApproximatelyEquals(left, right, epsilon);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ApproximatelyEquals_WithZeroOrNegativeEpsilon_ReturnsFalse()
    {
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(1d, 1d, 0d));
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(1d, 1d, -1d));
    }

    [TestMethod]
    public void ApproximatelyEquals_WithSpecialValues_ReturnsFalse()
    {
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(double.NaN, 1d));
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(1d, double.NaN));
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(double.NaN, double.NaN));
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(double.PositiveInfinity, double.PositiveInfinity));
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(double.NegativeInfinity, double.NegativeInfinity));
        Assert.IsFalse(NumericalEquality.ApproximatelyEquals(double.PositiveInfinity, double.NegativeInfinity));
    }

    [TestMethod]
    public void ApproximatelyEquals_WithIdenticalBoundaryValues_ReturnsTrue()
    {
        Assert.IsTrue(NumericalEquality.ApproximatelyEquals(double.MaxValue, double.MaxValue));
        Assert.IsTrue(NumericalEquality.ApproximatelyEquals(double.MinValue, double.MinValue));
        Assert.IsTrue(NumericalEquality.ApproximatelyEquals(double.Epsilon, double.Epsilon));
    }

    [TestMethod]
    public void ComparisonMethods_AreSymmetric()
    {
        Assert.AreEqual(
            NumericalEquality.Equals(1d, 1.00000000005d),
            NumericalEquality.Equals(1.00000000005d, 1d));

        Assert.AreEqual(
            NumericalEquality.Equals(1f, 1.0000005f),
            NumericalEquality.Equals(1.0000005f, 1f));

        Assert.AreEqual(
            NumericalEquality.ApproximatelyEquals(1d, 1.00000000005d),
            NumericalEquality.ApproximatelyEquals(1.00000000005d, 1d));
    }
}