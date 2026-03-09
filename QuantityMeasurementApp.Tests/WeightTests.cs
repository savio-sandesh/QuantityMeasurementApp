using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class WeightTests
    {
        private const double Epsilon = 0.000001;

        [TestMethod]
        public void GivenSameKilogramValue_WhenCompared_ShouldReturnTrue()
        {
            var first = new Weight(1.0, WeightUnit.Kilogram);
            var second = new Weight(1.0, WeightUnit.Kilogram);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void GivenOneKilogramAndThousandGrams_WhenCompared_ShouldReturnTrue()
        {
            var kilogram = new Weight(1.0, WeightUnit.Kilogram);
            var gram = new Weight(1000.0, WeightUnit.Gram);

            Assert.IsTrue(kilogram.Equals(gram));
            Assert.IsTrue(gram.Equals(kilogram));
        }

        [TestMethod]
        public void GivenOnePoundAndEquivalentGrams_WhenCompared_ShouldReturnTrue()
        {
            var pound = new Weight(1.0, WeightUnit.Pound);
            var grams = new Weight(453.592, WeightUnit.Gram);

            Assert.IsTrue(pound.Equals(grams));
        }

        [TestMethod]
        public void GivenDifferentValues_WhenCompared_ShouldReturnFalse()
        {
            var first = new Weight(1.0, WeightUnit.Kilogram);
            var second = new Weight(2.0, WeightUnit.Kilogram);

            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void GivenWeightComparedWithNull_ShouldReturnFalse()
        {
            var value = new Weight(1.0, WeightUnit.Kilogram);

            Assert.IsFalse(value.Equals(null));
        }

        [TestMethod]
        public void GivenWeightComparedWithLength_ShouldReturnFalse()
        {
            var weight = new Weight(1.0, WeightUnit.Kilogram);
            var length = new Length(1.0, LengthUnit.Feet);

            Assert.IsFalse(weight.Equals(length));
        }

        [TestMethod]
        public void GivenEquivalentWeights_WhenGetHashCodeCalled_ShouldMatch()
        {
            var first = new Weight(1.0, WeightUnit.Kilogram);
            var second = new Weight(1000.0, WeightUnit.Gram);

            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }

        [TestMethod]
        public void GivenPound_WhenConvertedToKilogram_ShouldReturnExpectedValue()
        {
            double result = Weight.Convert(2.20462, WeightUnit.Pound, WeightUnit.Kilogram);

            Assert.AreEqual(1.0, result, 0.00001);
        }

        [TestMethod]
        public void GivenKilogram_WhenConvertedToPound_ShouldReturnExpectedValue()
        {
            double result = Weight.Convert(1.0, WeightUnit.Kilogram, WeightUnit.Pound);

            Assert.AreEqual(2.2046244201837775, result, Epsilon);
        }

        [TestMethod]
        public void GivenGram_WhenConvertedToKilogram_ShouldReturnExpectedValue()
        {
            double result = Weight.Convert(500.0, WeightUnit.Gram, WeightUnit.Kilogram);

            Assert.AreEqual(0.5, result, Epsilon);
        }

        [TestMethod]
        public void GivenWeight_WhenRoundTrippedAcrossUnits_ShouldPreserveValue()
        {
            double value = 1.5;
            double grams = Weight.Convert(value, WeightUnit.Kilogram, WeightUnit.Gram);
            double pounds = Weight.Convert(grams, WeightUnit.Gram, WeightUnit.Pound);
            double kilograms = Weight.Convert(pounds, WeightUnit.Pound, WeightUnit.Kilogram);

            Assert.AreEqual(value, kilograms, 0.00001);
        }

        [TestMethod]
        public void GivenSameReference_WhenCompared_ShouldReturnTrue()
        {
            var value = new Weight(2.5, WeightUnit.Kilogram);

            Assert.IsTrue(value.Equals(value));
        }

        [TestMethod]
        public void GivenKilogramAndGram_WhenAdded_ShouldReturnSumInFirstUnit()
        {
            var first = new Weight(1.0, WeightUnit.Kilogram);
            var second = new Weight(1000.0, WeightUnit.Gram);

            var result = first.Add(second);

            Assert.AreEqual(2.0, result.ConvertTo(WeightUnit.Kilogram), Epsilon);
            Assert.AreEqual(WeightUnit.Kilogram, result.Unit);
        }

        [TestMethod]
        public void GivenPoundAndEquivalentGrams_WhenAddedWithPoundTarget_ShouldReturnExpectedValue()
        {
            var first = new Weight(1.0, WeightUnit.Pound);
            var second = new Weight(453.592, WeightUnit.Gram);

            var result = first.Add(second, WeightUnit.Pound);

            Assert.AreEqual(2.0, result.ConvertTo(WeightUnit.Pound), 0.00001);
            Assert.AreEqual(WeightUnit.Pound, result.Unit);
        }

        [TestMethod]
        public void GivenRawValues_WhenAddedWithOverload_ShouldReturnExpectedResult()
        {
            var result = Weight.Add(1.0, WeightUnit.Kilogram, 1000.0, WeightUnit.Gram, WeightUnit.Gram);

            Assert.AreEqual(2000.0, result.ConvertTo(WeightUnit.Gram), Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void GivenStaticTwoOperandAdd_WhenAdded_ShouldReturnSumInFirstUnit()
        {
            var first = new Weight(1.0, WeightUnit.Pound);
            var second = new Weight(453.592, WeightUnit.Gram);

            var result = Weight.Add(first, second);

            Assert.AreEqual(2.0, result.ConvertTo(WeightUnit.Pound), 0.00001);
            Assert.AreEqual(WeightUnit.Pound, result.Unit);
        }

        [TestMethod]
        public void GivenZeroAndNegativeValues_WhenAdded_ShouldReturnExpectedValue()
        {
            var result = Weight.Add(new Weight(5.0, WeightUnit.Kilogram), new Weight(-2000.0, WeightUnit.Gram), WeightUnit.Kilogram);

            Assert.AreEqual(3.0, result.ConvertTo(WeightUnit.Kilogram), Epsilon);
        }

        [TestMethod]
        public void GivenNaNValue_WhenConstructed_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Weight(double.NaN, WeightUnit.Kilogram));
        }

        [TestMethod]
        public void GivenInfiniteValue_WhenConstructed_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Weight(double.PositiveInfinity, WeightUnit.Kilogram));
        }

        [TestMethod]
        public void GivenInvalidUnit_WhenConstructed_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Weight(1.0, (WeightUnit)999));
        }

        [TestMethod]
        public void GivenNullOperand_WhenAdded_ShouldThrowArgumentNullException()
        {
            var first = new Weight(1.0, WeightUnit.Kilogram);

            Assert.ThrowsException<ArgumentNullException>(() => first.Add(null!));
        }

        [TestMethod]
        public void GivenWeightUnitPound_WhenConvertingToBase_ShouldReturnExpectedValue()
        {
            double result = WeightUnit.Pound.ConvertToBaseUnit(2.0);

            Assert.AreEqual(0.907184, result, Epsilon);
        }

        [TestMethod]
        public void GivenWeightUnitGram_WhenConvertingFromBase_ShouldReturnExpectedValue()
        {
            double result = WeightUnit.Gram.ConvertFromBaseUnit(1.0);

            Assert.AreEqual(1000.0, result, Epsilon);
        }
    }
}
