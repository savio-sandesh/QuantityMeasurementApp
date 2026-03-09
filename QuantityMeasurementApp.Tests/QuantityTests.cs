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
        public void GivenVolumeUnits_WhenComparedAcrossUnits_ShouldReturnTrue()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var millilitre = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            Assert.IsTrue(litre.Equals(millilitre));
        }

        [TestMethod]
        public void GivenLitreAndEquivalentGallon_WhenCompared_ShouldReturnTrue()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var gallon = new Quantity<VolumeUnit>(0.2641720523581484, VolumeUnit.Gallon);

            Assert.IsTrue(litre.Equals(gallon));
        }

        [TestMethod]
        public void GivenCrossCategory_WhenComparedAsObject_ShouldReturnFalse()
        {
            object length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            object weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.IsFalse(length.Equals(weight));
        }

        [TestMethod]
        public void GivenVolumeAndLength_WhenComparedAsObject_ShouldReturnFalse()
        {
            object volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            object length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);

            Assert.IsFalse(volume.Equals(length));
        }

        [TestMethod]
        public void GivenVolumeAndWeight_WhenComparedAsObject_ShouldReturnFalse()
        {
            object volume = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            object weight = new Quantity<WeightUnit>(1.0, WeightUnit.Kilogram);

            Assert.IsFalse(volume.Equals(weight));
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
        public void GivenVolumeQuantity_WhenConverted_ShouldReturnExpectedUnitAndValue()
        {
            var litre = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            var millilitre = litre.ConvertTo(VolumeUnit.Millilitre);

            Assert.AreEqual(1000.0, millilitre.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Millilitre, millilitre.Unit);
        }

        [TestMethod]
        public void GivenGallon_WhenConvertedToLitre_ShouldReturnExpectedValue()
        {
            var gallon = new Quantity<VolumeUnit>(1.0, VolumeUnit.Gallon);

            var litre = gallon.ConvertTo(VolumeUnit.Litre);

            Assert.AreEqual(3.78541, litre.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, litre.Unit);
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
        public void GivenVolumeAddition_WhenNoTargetSpecified_ShouldReturnFirstUnit()
        {
            var first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var second = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = first.Add(second);

            Assert.AreEqual(2.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        [TestMethod]
        public void GivenVolumeAddition_WhenTargetSpecified_ShouldReturnTargetUnit()
        {
            var first = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var second = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);

            var result = first.Add(second, VolumeUnit.Gallon);

            Assert.AreEqual(0.5283441047162968, result.Value, 0.00001);
            Assert.AreEqual(VolumeUnit.Gallon, result.Unit);
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

        [TestMethod]
        public void GivenVolumeUnit_WhenUsingConversionExtensions_ShouldReturnExpectedValues()
        {
            Assert.AreEqual(1.0, VolumeUnit.Litre.GetConversionFactor(), Epsilon);
            Assert.AreEqual(0.001, VolumeUnit.Millilitre.GetConversionFactor(), Epsilon);
            Assert.AreEqual(3.78541, VolumeUnit.Gallon.GetConversionFactor(), Epsilon);

            Assert.AreEqual(1.0, VolumeUnit.Millilitre.ConvertToBaseUnit(1000.0), Epsilon);
            Assert.AreEqual(1000.0, VolumeUnit.Millilitre.ConvertFromBaseUnit(1.0), Epsilon);
        }

        [TestMethod]
        public void GivenLengthSubtraction_WhenCrossUnit_ShouldReturnExpectedResultInFirstUnit()
        {
            var feet = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var inches = new Quantity<LengthUnit>(6.0, LengthUnit.Inch);

            var result = feet.Subtract(inches);

            Assert.AreEqual(9.5, result.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [TestMethod]
        public void GivenWeightSubtraction_WhenTargetSpecified_ShouldReturnExpectedTargetUnit()
        {
            var kilograms = new Quantity<WeightUnit>(10.0, WeightUnit.Kilogram);
            var grams = new Quantity<WeightUnit>(5000.0, WeightUnit.Gram);

            var result = kilograms.Subtract(grams, WeightUnit.Gram);

            Assert.AreEqual(5000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void GivenVolumeSubtraction_WhenResultNegative_ShouldPreserveSign()
        {
            var small = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);
            var large = new Quantity<VolumeUnit>(2.0, VolumeUnit.Litre);

            var result = small.Subtract(large);

            Assert.AreEqual(-1.0, result.Value, Epsilon);
            Assert.AreEqual(VolumeUnit.Litre, result.Unit);
        }

        [TestMethod]
        public void GivenEquivalentQuantities_WhenSubtracted_ShouldReturnZero()
        {
            var feet = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var inches = new Quantity<LengthUnit>(120.0, LengthUnit.Inch);

            var result = feet.Subtract(inches);

            Assert.AreEqual(0.0, result.Value, Epsilon);
        }

        [TestMethod]
        public void GivenSubtraction_WhenOrderSwapped_ShouldBeNonCommutative()
        {
            var a = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var b = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var first = a.Subtract(b);
            var second = b.Subtract(a);

            Assert.AreEqual(5.0, first.Value, Epsilon);
            Assert.AreEqual(-5.0, second.Value, Epsilon);
        }

        [TestMethod]
        public void GivenDivision_WhenSameUnit_ShouldReturnExpectedRatio()
        {
            var first = new Quantity<WeightUnit>(10.0, WeightUnit.Kilogram);
            var second = new Quantity<WeightUnit>(5.0, WeightUnit.Kilogram);

            double ratio = first.Divide(second);

            Assert.AreEqual(2.0, ratio, Epsilon);
        }

        [TestMethod]
        public void GivenDivision_WhenCrossUnitSameCategory_ShouldReturnExpectedRatio()
        {
            var inches = new Quantity<LengthUnit>(24.0, LengthUnit.Inch);
            var feet = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            double ratio = inches.Divide(feet);

            Assert.AreEqual(1.0, ratio, Epsilon);
        }

        [TestMethod]
        public void GivenDivision_WhenOrderSwapped_ShouldBeNonCommutative()
        {
            var a = new Quantity<VolumeUnit>(10.0, VolumeUnit.Litre);
            var b = new Quantity<VolumeUnit>(5.0, VolumeUnit.Litre);

            double first = a.Divide(b);
            double second = b.Divide(a);

            Assert.AreEqual(2.0, first, Epsilon);
            Assert.AreEqual(0.5, second, Epsilon);
        }

        [TestMethod]
        public void GivenDivision_WhenDivisorIsZero_ShouldThrowArithmeticException()
        {
            var first = new Quantity<WeightUnit>(10.0, WeightUnit.Kilogram);
            var zero = new Quantity<WeightUnit>(0.0, WeightUnit.Kilogram);

            Assert.ThrowsException<ArithmeticException>(() => first.Divide(zero));
        }

        [TestMethod]
        public void GivenNullOther_WhenSubtracted_ShouldThrowArgumentNullException()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            Assert.ThrowsException<ArgumentNullException>(() => first.Subtract(null!));
        }

        [TestMethod]
        public void GivenNullOther_WhenDivided_ShouldThrowArgumentNullException()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);

            Assert.ThrowsException<ArgumentNullException>(() => first.Divide(null!));
        }

        [TestMethod]
        public void GivenSubtractionAndDivision_WhenApplied_ShouldKeepOperandsImmutable()
        {
            var first = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var second = new Quantity<LengthUnit>(6.0, LengthUnit.Inch);

            _ = first.Subtract(second);
            _ = first.Divide(new Quantity<LengthUnit>(2.0, LengthUnit.Feet));

            Assert.AreEqual(10.0, first.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Feet, first.Unit);
            Assert.AreEqual(6.0, second.Value, Epsilon);
            Assert.AreEqual(LengthUnit.Inch, second.Unit);
        }

        [TestMethod]
        public void GivenStaticSubtractWithTarget_WhenCalled_ShouldReturnExpectedResult()
        {
            var first = new Quantity<WeightUnit>(10.0, WeightUnit.Kilogram);
            var second = new Quantity<WeightUnit>(2000.0, WeightUnit.Gram);

            var result = Quantity<WeightUnit>.Subtract(first, second, WeightUnit.Gram);

            Assert.AreEqual(8000.0, result.Value, Epsilon);
            Assert.AreEqual(WeightUnit.Gram, result.Unit);
        }

        [TestMethod]
        public void GivenStaticDivide_WhenCalled_ShouldReturnExpectedRatio()
        {
            var first = new Quantity<VolumeUnit>(1000.0, VolumeUnit.Millilitre);
            var second = new Quantity<VolumeUnit>(1.0, VolumeUnit.Litre);

            double ratio = Quantity<VolumeUnit>.Divide(first, second);

            Assert.AreEqual(1.0, ratio, Epsilon);
        }

        [TestMethod]
        public void GivenArithmeticChain_WhenExecuted_ShouldReturnExpectedResult()
        {
            var start = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var add = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            var subtract = new Quantity<LengthUnit>(6.0, LengthUnit.Inch);
            var divisor = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);

            var composed = start.Add(add).Subtract(subtract);
            double ratio = composed.Divide(divisor);

            Assert.AreEqual(2.1, ratio, Epsilon);
        }
    }
}
