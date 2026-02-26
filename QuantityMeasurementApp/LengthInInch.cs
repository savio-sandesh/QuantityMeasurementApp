using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Represents an immutable length measurement in inches.
    /// Supports value-based equality and cross-unit comparison with feet.
    /// </summary>
    public class LengthInInch
    {
        // Immutable storage of the measurement value in inches
        private readonly double value;

        // Tolerance to handle floating-point precision issues
        private const double Tolerance = 0.0001;

        /// <summary>
        /// Gets the measurement value in inches.
        /// </summary>
        public double Value => value;

        /// <summary>
        /// Initializes a new instance of LengthInInch.
        /// </summary>
        /// <param name="value">Measurement value in inches.</param>
        public LengthInInch(double value)
        {
            this.value = value;
        }

        /// <summary>
        /// Determines equality with another measurement.
        /// Supports Inch-to-Inch and Inch-to-Feet comparison.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            // Inch-to-Inch comparison
            if (obj is LengthInInch otherInch)
                return Math.Abs(this.value - otherInch.value) <= Tolerance;

            // Inch-to-Feet comparison (convert feet to inches)
            if (obj is LengthInFeet otherFeet)
                return Math.Abs((this.value / 12) - otherFeet.Value) <= Tolerance;

            return false;
        }

        /// <summary>
        /// Returns a hash code consistent with the equality logic.
        /// </summary>
        public override int GetHashCode()
        {
            double normalized = Math.Round(value / Tolerance) * Tolerance;
            return normalized.GetHashCode();
        }
    }
}