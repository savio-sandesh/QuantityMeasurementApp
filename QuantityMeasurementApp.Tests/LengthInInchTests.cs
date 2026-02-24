using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Unit tests validating equality behavior of length measurements in inches.
    /// Covers Inch-to-Inch and Inch-to-Feet comparison scenarios (UC2).
    /// </summary>
    [TestClass]
    public class LengthInInchTests
    {
        // ======================
        // Inch-to-Inch Equality
        // ======================

        [TestMethod]
        public void GivenSameInchValue_WhenCompared_ShouldReturnTrue()
        {
            var first = new LengthInInch(12.0);
            var second = new LengthInInch(12.0);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void GivenDifferentInchValue_WhenCompared_ShouldReturnFalse()
        {
            var first = new LengthInInch(12.0);
            var second = new LengthInInch(10.0);

            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void GivenInchComparedWithNull_ShouldReturnFalse()
        {
            var value = new LengthInInch(12.0);

            Assert.IsFalse(value.Equals(null));
        }

        [TestMethod]
        public void GivenInchComparedWithDifferentType_ShouldReturnFalse()
        {
            var value = new LengthInInch(12.0);

            Assert.IsFalse(value.Equals("12"));
        }

        // ======================
        // Cross Unit Equality
        // ======================

        [TestMethod]
        public void GivenOneFeetAndTwelveInches_WhenCompared_ShouldReturnTrue()
        {
            var feet = new LengthInFeet(1.0);
            var inch = new LengthInInch(12.0);

            Assert.IsTrue(feet.Equals(inch));
        }

        [TestMethod]
        public void GivenNonEquivalentFeetAndInches_WhenCompared_ShouldReturnFalse()
        {
            var feet = new LengthInFeet(1.0);
            var inch = new LengthInInch(10.0);

            Assert.IsFalse(feet.Equals(inch));
        }
    }
}