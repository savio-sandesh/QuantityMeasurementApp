using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityTests
    {
        private const double Epsilon = 0.000001;

        [TestMethod]
        public void GivenLengthUnits_WhenComparedAcrossUnits_ShouldReturnTrue()
        {
            var feet = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var inches = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            Assert.IsTrue(feet.Equals(inches));
        }

        [TestMethod]
        public void GivenWeightUnits_WhenComparedAcrossUnits_ShouldReturnTrue()
        {
            var kilogram = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var grams = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            Assert.IsTrue(kilogram.Equals(grams));
        }

        [TestMethod]
        public void GivenCrossCategory_WhenComparedAsObject_ShouldReturnFalse()
        {
            object length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            object weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.IsFalse(length.Equals(weight));
        }

        [TestMethod]
        public void GivenLengthQuantity_WhenConverted_ShouldReturnExpectedUnitAndValue()
        {
            var feet = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            var inches = feet.ConvertTo(LengthUnit.Inch);

            Assert.AreEqual(12.0, inches.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, inches.Unit);
        }

        [TestMethod]
        public void GivenWeightQuantity_WhenConverted_ShouldReturnExpectedUnitAndValue()
        {
            var kilogram = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            var grams = kilogram.ConvertTo(WeightUnit.Gram);

            Assert.AreEqual(1000.0, grams.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, grams.Unit);
        }

        [TestMethod]
        public void GivenLengthAddition_WhenNoTargetSpecified_ShouldReturnFirstUnit()
        {
            var first = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var second = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            var result = first.Add(second);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void GivenWeightAddition_WhenTargetSpecified_ShouldReturnTargetUnit()
        {
            var first = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var second = new Quantity<WeightUnit>(1000.0, WeightUnit.Gram);

            var result = first.Add(second, WeightUnit.Gram);

            Assert.AreEqual(2000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void GivenEqualGenericQuantities_WhenGetHashCodeCalled_ShouldMatch()
        {
            var first = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var second = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }

        [TestMethod]
        public void GivenNullUnitValue_WhenConstructed_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Quantity<LengthUnit>(1.0, (LengthUnit)999));
        }

        [TestMethod]
        public void GivenNonFiniteValue_WhenConstructed_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Quantity<WeightUnit>(double.NaN, WeightUnit.Kilogram));
        }

        [TestMethod]
        public void GivenNullOther_WhenAdded_ShouldThrowArgumentNullException()
        {
            var first = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.ThrowsException<ArgumentNullException>(() => first.Add(null!));
        }

        [TestMethod]
        public void GivenStaticAddWithTarget_WhenCalled_ShouldReturnExpectedResult()
        {
            var first = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);
            var second = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            var result = Quantity<WeightUnit>.Add(first, second, WeightUnit.Kilogram);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void GivenToString_WhenCalled_ShouldContainQuantityAndUnit()
        {
            var value = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            string text = value.ToString();

            StringAssert.Contains(text, "Quantity(");
            StringAssert.Contains(text, "Kilogram");
        }
    }
}
