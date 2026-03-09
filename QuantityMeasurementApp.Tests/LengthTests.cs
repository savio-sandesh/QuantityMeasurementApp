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

        [TestMethod]
        public void GivenSameYardValue_WhenCompared_ShouldReturnTrue()
        {
            var first = new Length(2.0, LengthUnit.Yard);
            var second = new Length(2.0, LengthUnit.Yard);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void GivenOneYardAndThreeFeet_WhenCompared_ShouldReturnTrue()
        {
            var yard = new Length(1.0, LengthUnit.Yard);
            var feet = new Length(3.0, LengthUnit.Feet);

            Assert.IsTrue(yard.Equals(feet));
        }

        [TestMethod]
        public void GivenOneYardAndThirtySixInches_WhenCompared_ShouldReturnTrue()
        {
            var yard = new Length(1.0, LengthUnit.Yard);
            var inch = new Length(36.0, LengthUnit.Inch);

            Assert.IsTrue(yard.Equals(inch));
        }

        [TestMethod]
        public void GivenOneCentimeterAndEquivalentInches_WhenCompared_ShouldReturnTrue()
        {
            var centimeter = new Length(1.0, LengthUnit.Centimeter);
            var inch = new Length(0.393701, LengthUnit.Inch);

            Assert.IsTrue(centimeter.Equals(inch));
        }

        [TestMethod]
        public void GivenCentimeterAndFeetNonEquivalent_WhenCompared_ShouldReturnFalse()
        {
            var centimeter = new Length(1.0, LengthUnit.Centimeter);
            var feet = new Length(1.0, LengthUnit.Feet);

            Assert.IsFalse(centimeter.Equals(feet));
        }

        [TestMethod]
        public void GivenMultiUnitEquivalentValues_WhenCompared_ShouldReturnTrue()
        {
            var yard = new Length(2.0, LengthUnit.Yard);
            var feet = new Length(6.0, LengthUnit.Feet);
            var inch = new Length(72.0, LengthUnit.Inch);

            Assert.IsTrue(yard.Equals(feet));
            Assert.IsTrue(feet.Equals(inch));
            Assert.IsTrue(yard.Equals(inch));
        }
    }
}