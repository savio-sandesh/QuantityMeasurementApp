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
            ValidateFinite(value);
            ValidateUnit(unit, nameof(unit));

            Unit = unit;

            // Convert to base unit (Feet)
            valueInFeet = value * unit.ToFeetFactor();
        }

        /// <summary>
        /// Converts a numeric length from one unit to another.
        /// </summary>
        /// <param name="value">Numeric value to convert.</param>
        /// <param name="sourceUnit">Source unit.</param>
        /// <param name="targetUnit">Target unit.</param>
        /// <returns>Converted value in target unit.</returns>
        /// <exception cref="ArgumentException">Thrown when value is not finite or when units are invalid.</exception>
        public static double Convert(double value, LengthUnit sourceUnit, LengthUnit targetUnit)
        {
            ValidateFinite(value);
            ValidateUnit(sourceUnit, nameof(sourceUnit));
            ValidateUnit(targetUnit, nameof(targetUnit));

            double valueInFeet = value * sourceUnit.ToFeetFactor();
            return valueInFeet / targetUnit.ToFeetFactor();
        }

        /// <summary>
        /// Converts this measurement to the requested unit and returns the numeric value.
        /// </summary>
        public double ConvertTo(LengthUnit targetUnit)
        {
            ValidateUnit(targetUnit, nameof(targetUnit));
            return valueInFeet / targetUnit.ToFeetFactor();
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

        private static void ValidateFinite(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Value must be a finite number", nameof(value));
            }
        }

        private static void ValidateUnit(LengthUnit unit, string parameterName)
        {
            if (!unit.IsDefined())
            {
                throw new ArgumentException("Unsupported length unit", parameterName);
            }
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