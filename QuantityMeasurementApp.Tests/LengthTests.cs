using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Unit tests validating generic Length equality behavior.
    /// </summary>
    [TestClass]
    public class LengthTests
    {
        [TestMethod]
        public void GivenSameFeetValue_WhenCompared_ShouldReturnTrue()
        {
            var first = new Length(1.0, LengthUnit.Feet);
            var second = new Length(1.0, LengthUnit.Feet);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void GivenSameInchValue_WhenCompared_ShouldReturnTrue()
        {
            var first = new Length(12.0, LengthUnit.Inch);
            var second = new Length(12.0, LengthUnit.Inch);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void GivenOneFeetAndTwelveInches_WhenCompared_ShouldReturnTrue()
        {
            var feet = new Length(1.0, LengthUnit.Feet);
            var inch = new Length(12.0, LengthUnit.Inch);

            Assert.IsTrue(feet.Equals(inch));
        }

        [TestMethod]
        public void GivenDifferentValues_WhenCompared_ShouldReturnFalse()
        {
            var feet = new Length(1.0, LengthUnit.Feet);
            var inch = new Length(10.0, LengthUnit.Inch);

            Assert.IsFalse(feet.Equals(inch));
        }

        [TestMethod]
        public void GivenLengthComparedWithNull_ShouldReturnFalse()
        {
            var value = new Length(1.0, LengthUnit.Feet);

            Assert.IsFalse(value.Equals(null));
        }
    }
}