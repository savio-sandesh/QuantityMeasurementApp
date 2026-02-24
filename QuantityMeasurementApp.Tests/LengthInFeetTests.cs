using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Unit tests validating equality behavior of LengthInFeet value object.
    /// Ensures compliance with equality contract principles.
    /// </summary>
    [TestClass]
    public class LengthInFeetTests
    {
        [TestMethod]
        public void Equals_ShouldReturnTrue_ForSameValue()
        {
            var first = new LengthInFeet(1.0);
            var second = new LengthInFeet(1.0);

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse_ForDifferentValue()
        {
            var first = new LengthInFeet(1.0);
            var second = new LengthInFeet(2.0);

            Assert.IsFalse(first.Equals(second));
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse_WhenComparedWithNull()
        {
            var value = new LengthInFeet(1.0);

            Assert.IsFalse(value.Equals(null));
        }

        [TestMethod]
        public void Equals_ShouldReturnTrue_ForSameReference()
        {
            var value = new LengthInFeet(1.0);

            Assert.IsTrue(value.Equals(value));
        }

        [TestMethod]
        public void Equals_ShouldReturnFalse_ForDifferentType()
        {
            var value = new LengthInFeet(1.0);
            object other = "DifferentType";

            Assert.IsFalse(value.Equals(other));
        }

        [TestMethod]
        public void GetHashCode_ShouldMatch_ForEqualValues()
        {
            var first = new LengthInFeet(1.0);
            var second = new LengthInFeet(1.0);

            Assert.IsTrue(first.Equals(second));
            Assert.AreEqual(first.GetHashCode(), second.GetHashCode());
        }
    }
}