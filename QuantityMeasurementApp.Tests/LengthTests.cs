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
        private const double Epsilon = 0.000001;

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

        [TestMethod]
        public void GivenFeet_WhenConvertedToInch_ShouldReturnExpectedValue()
        {
            double result = Length.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch);

            Assert.AreEqual(12.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenInch_WhenConvertedToFeet_ShouldReturnExpectedValue()
        {
            double result = Length.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenYard_WhenConvertedToInch_ShouldReturnExpectedValue()
        {
            double result = Length.Convert(1.0, LengthUnit.Yard, LengthUnit.Inch);

            Assert.AreEqual(36.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenInch_WhenConvertedToYard_ShouldReturnExpectedValue()
        {
            double result = Length.Convert(72.0, LengthUnit.Inch, LengthUnit.Yard);

            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenCentimeter_WhenConvertedToInch_ShouldReturnExpectedValue()
        {
            double result = Length.Convert(2.54, LengthUnit.Centimeter, LengthUnit.Inch);

            Assert.AreEqual(1.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenFeet_WhenConvertedToYard_ShouldReturnExpectedValue()
        {
            double result = Length.Convert(6.0, LengthUnit.Feet, LengthUnit.Yard);

            Assert.AreEqual(2.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenValue_WhenRoundTrippedAcrossUnits_ShouldPreserveValue()
        {
            double value = 5.25;
            double inches = Length.Convert(value, LengthUnit.Feet, LengthUnit.Inch);
            double feet = Length.Convert(inches, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(value, feet, Epsilon);
        }

        [TestMethod]
        public void GivenZeroValue_WhenConverted_ShouldReturnZero()
        {
            double result = Length.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch);

            Assert.AreEqual(0.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenNegativeValue_WhenConverted_ShouldPreserveSign()
        {
            double result = Length.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch);

            Assert.AreEqual(-12.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenSameSourceAndTargetUnit_WhenConverted_ShouldReturnOriginalValue()
        {
            double result = Length.Convert(5.0, LengthUnit.Feet, LengthUnit.Feet);

            Assert.AreEqual(5.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenInvalidUnit_WhenConverted_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                Length.Convert(1.0, (LengthUnit)999, LengthUnit.Feet));
        }

        [TestMethod]
        public void GivenNaNValue_WhenConverted_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                Length.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inch));
        }

        [TestMethod]
        public void GivenInfiniteValue_WhenConverted_ShouldThrowArgumentException()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                Length.Convert(double.PositiveInfinity, LengthUnit.Feet, LengthUnit.Inch));
        }

        [TestMethod]
        public void GivenLengthInstance_WhenConvertedToTargetUnit_ShouldReturnExpectedValue()
        {
            var length = new Length(1.0, LengthUnit.Yard);

            double result = length.ConvertTo(LengthUnit.Inch);

            Assert.AreEqual(36.0, result, Epsilon);
        }

        [TestMethod]
        public void GivenFeetAndFeet_WhenAdded_ShouldReturnSumInFeet()
        {
            var first = new Length(1.0, LengthUnit.Feet);
            var second = new Length(2.0, LengthUnit.Feet);

            var result = first.Add(second);

            Assert.AreEqual(3.0, result.ConvertTo(LengthUnit.Feet), Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void GivenInchAndInch_WhenAdded_ShouldReturnSumInInch()
        {
            var first = new Length(6.0, LengthUnit.Inch);
            var second = new Length(6.0, LengthUnit.Inch);

            var result = first.Add(second);

            Assert.AreEqual(12.0, result.ConvertTo(LengthUnit.Inch), Epsilon);
            Assert.AreEqual(LengthUnit.Inch, result.Unit);
        }

        [TestMethod]
        public void GivenFeetAndInch_WhenAdded_ShouldReturnSumInFirstOperandUnit()
        {
            var first = new Length(1.0, LengthUnit.Feet);
            var second = new Length(12.0, LengthUnit.Inch);

            var result = first.Add(second);

            Assert.AreEqual(2.0, result.ConvertTo(LengthUnit.Feet), Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void GivenInchAndFeet_WhenAdded_ShouldReturnSumInFirstOperandUnit()
        {
            var first = new Length(12.0, LengthUnit.Inch);
            var second = new Length(1.0, LengthUnit.Feet);

            var result = first.Add(second);

            Assert.AreEqual(24.0, result.ConvertTo(LengthUnit.Inch), Epsilon);
            Assert.AreEqual(LengthUnit.Inch, result.Unit);
        }

        [TestMethod]
        public void GivenYardAndFeet_WhenAdded_ShouldReturnExpectedSum()
        {
            var result = Length.Add(new Length(1.0, LengthUnit.Yard), new Length(3.0, LengthUnit.Feet), LengthUnit.Yard);

            Assert.AreEqual(2.0, result.ConvertTo(LengthUnit.Yard), Epsilon);
            Assert.AreEqual(LengthUnit.Yard, result.Unit);
        }

        [TestMethod]
        public void GivenCentimeterAndInch_WhenAdded_ShouldReturnExpectedSumInCentimeter()
        {
            var first = new Length(2.54, LengthUnit.Centimeter);
            var second = new Length(1.0, LengthUnit.Inch);

            var result = first.Add(second);

            Assert.AreEqual(5.08, result.ConvertTo(LengthUnit.Centimeter), 0.00001);
            Assert.AreEqual(LengthUnit.Centimeter, result.Unit);
        }

        [TestMethod]
        public void GivenEquivalentOperands_WhenAddedWithDifferentOrder_ShouldBeCommutative()
        {
            var first = new Length(1.0, LengthUnit.Feet);
            var second = new Length(12.0, LengthUnit.Inch);

            var resultOne = Length.Add(first, second, LengthUnit.Feet);
            var resultTwo = Length.Add(second, first, LengthUnit.Feet);

            Assert.AreEqual(resultOne.ConvertTo(LengthUnit.Feet), resultTwo.ConvertTo(LengthUnit.Feet), Epsilon);
        }

        [TestMethod]
        public void GivenZeroLength_WhenAdded_ShouldBehaveAsIdentity()
        {
            var first = new Length(5.0, LengthUnit.Feet);
            var zero = new Length(0.0, LengthUnit.Inch);

            var result = first.Add(zero);

            Assert.AreEqual(5.0, result.ConvertTo(LengthUnit.Feet), Epsilon);
        }

        [TestMethod]
        public void GivenNegativeLength_WhenAdded_ShouldReturnExpectedSum()
        {
            var first = new Length(5.0, LengthUnit.Feet);
            var second = new Length(-2.0, LengthUnit.Feet);

            var result = first.Add(second);

            Assert.AreEqual(3.0, result.ConvertTo(LengthUnit.Feet), Epsilon);
        }

        [TestMethod]
        public void GivenNullSecondOperand_WhenAdded_ShouldThrowArgumentNullException()
        {
            var first = new Length(1.0, LengthUnit.Feet);

            Assert.ThrowsException<ArgumentNullException>(() => first.Add(null!));
        }

        [TestMethod]
        public void GivenLargeValues_WhenAdded_ShouldReturnExpectedSum()
        {
            var first = new Length(1_000_000.0, LengthUnit.Feet);
            var second = new Length(1_000_000.0, LengthUnit.Feet);

            var result = first.Add(second);

            Assert.AreEqual(2_000_000.0, result.ConvertTo(LengthUnit.Feet), Epsilon);
        }

        [TestMethod]
        public void GivenSmallValues_WhenAdded_ShouldReturnExpectedSum()
        {
            var first = new Length(0.001, LengthUnit.Feet);
            var second = new Length(0.002, LengthUnit.Feet);

            var result = first.Add(second);

            Assert.AreEqual(0.003, result.ConvertTo(LengthUnit.Feet), Epsilon);
        }

        [TestMethod]
        public void GivenRawValues_WhenAddedWithOverload_ShouldReturnExpectedResult()
        {
            var result = Length.Add(1.0, LengthUnit.Feet, 12.0, LengthUnit.Inch, LengthUnit.Feet);

            Assert.AreEqual(2.0, result.ConvertTo(LengthUnit.Feet), Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }
    }
}