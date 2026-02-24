using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Represents a length measurement expressed in feet.
    /// 
    /// This is an immutable value object that implements 
    /// value-based equality comparison.
    /// 
    /// Two instances are considered equal if their numeric
    /// values are exactly equal.
    /// </summary>
    public sealed class LengthInFeet
    {
        // Stores the numeric value of the measurement.
        // Readonly ensures immutability after construction.
        private readonly double measurementValue;

        /// <summary>
        /// Initializes a new instance of LengthInFeet.
        /// </summary>
        /// <param name="value">The numeric length value in feet.</param>
        public LengthInFeet(double value)
        {
            measurementValue = value;
        }

        /// <summary>
        /// Provides value-based equality comparison.
        /// Ensures compliance with equality contract rules.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if both objects represent equal feet values.</returns>
        public override bool Equals(object? obj)
        {
            // Reference equality check (Reflexive property)
            if (ReferenceEquals(this, obj))
                return true;

            // Null and type safety check
            if (obj is null || obj.GetType() != typeof(LengthInFeet))
                return false;

            LengthInFeet other = (LengthInFeet)obj;

            // Direct double comparison
            return this.measurementValue.Equals(other.measurementValue);
        }

        /// <summary>
        /// Returns a hash code consistent with Equals.
        /// Required whenever Equals is overridden.
        /// </summary>
        public override int GetHashCode()
        {
            return measurementValue.GetHashCode();
        }
    }
}