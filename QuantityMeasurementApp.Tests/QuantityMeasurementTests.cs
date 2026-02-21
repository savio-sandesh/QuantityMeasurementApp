using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Unit tests validating equality behavior of Feet measurement.
    /// </summary>
    [TestClass]
    public class QuantityMeasurementTests
    {
        /// <summary>
        /// Verifies that two Feet objects with same value are equal.
        /// </summary>
        [TestMethod]
        public void testEquality_SameValue()
        {
            var first = new QuantityMeasurement.Feet(1.0);
            var second = new QuantityMeasurement.Feet(1.0);

            Assert.IsTrue(first.Equals(second), "Expected values to be equal.");
        }

        /// <summary>
        /// Verifies that two Feet objects with different values are not equal.
        /// </summary>
        [TestMethod]
        public void testEquality_DifferentValue()
        {
            var first = new QuantityMeasurement.Feet(1.0);
            var second = new QuantityMeasurement.Feet(2.0);

            Assert.IsFalse(first.Equals(second), "Expected values to be different.");
        }

        /// <summary>
        /// Verifies that comparison with null returns false.
        /// </summary>
        [TestMethod]
        public void testEquality_NullComparison()
        {
            var first = new QuantityMeasurement.Feet(1.0);

            Assert.IsFalse(first.Equals(null), "Value should not be equal to null.");
        }

        /// <summary>
        /// Verifies reflexive property (object equals itself).
        /// </summary>
        [TestMethod]
        public void testEquality_SameReference()
        {
            var first = new QuantityMeasurement.Feet(1.0);

            Assert.IsTrue(first.Equals(first), "Same reference must be equal.");
        }

        /// <summary>
        /// Verifies equality fails when comparing different types.
        /// </summary>
        [TestMethod]
        public void testEquality_DifferentType()
        {
            var first = new QuantityMeasurement.Feet(1.0);

            Assert.IsFalse(first.Equals("1.0"), "Feet should not equal a different type.");
        }
    }
}