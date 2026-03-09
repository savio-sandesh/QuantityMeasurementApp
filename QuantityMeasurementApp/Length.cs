using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Represents a generic immutable length measurement.
    /// Supports multiple units (Feet, Inch) and
    /// provides unit-safe equality comparison.
    /// </summary>
    public class Length
    {
        // Internal value stored in base unit (Feet)
        private readonly double valueInFeet;

        // Floating-point tolerance for safe equality comparison
        private const double Tolerance = 0.0001;

        /// <summary>
        /// Gets the original unit of the measurement.
        /// </summary>
        public LengthUnit Unit { get; }

        /// <summary>
        /// Initializes a new Length instance.
        /// Converts value to base unit (Feet) internally.
        /// </summary>
        /// <param name="value">Measurement value.</param>
        /// <param name="unit">Unit of measurement.</param>
        public Length(double value, LengthUnit unit)
        {
            Unit = unit;

            // Convert to base unit (Feet)
            valueInFeet = unit switch
            {
                LengthUnit.Feet => value,
                LengthUnit.Inch => value / 12,
                LengthUnit.Yard => value * 3,
                LengthUnit.Centimeter => (value * 0.393701) / 12,
                _ => throw new ArgumentException("Unsupported length unit")
            };
        }

        /// <summary>
        /// Determines equality between two Length objects.
        /// Comparison is performed using base unit conversion.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (obj is not Length other) return false;

            return Math.Abs(this.valueInFeet - other.valueInFeet) <= Tolerance;
        }

        /// <summary>
        /// Returns a hash code consistent with equality logic.
        /// </summary>
        public override int GetHashCode()
        {
            double normalized = Math.Round(valueInFeet / Tolerance) * Tolerance;
            return normalized.GetHashCode();
        }
    }

    /// <summary>
    /// Supported length units.
    /// </summary>
    // public enum LengthUnit
    // {
    //     Feet,
    //     Inch
    // }
}