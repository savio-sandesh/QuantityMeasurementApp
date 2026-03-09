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
    }
}
