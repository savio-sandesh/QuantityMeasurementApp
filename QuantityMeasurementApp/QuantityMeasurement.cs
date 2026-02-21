using System;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Main container class for quantity measurements.
    /// UC1 focuses on equality comparison for Feet measurement.
    /// </summary>
    public class QuantityMeasurement
    {
        /// <summary>
        /// Represents an immutable measurement in Feet.
        /// Implements value-based equality.
        /// </summary>
        public class Feet
        {
            // Stores the measurement value in feet.
            // Marked readonly to ensure immutability.
            private readonly double value;

            /// <summary>
            /// Initializes a new Feet measurement.
            /// </summary>
            /// <param name="value">Measurement value in feet.</param>
            public Feet(double value)
            {
                this.value = value;
            }

            /// <summary>
            /// Overrides Equals to provide value-based comparison.
            /// Follows equality contract rules.
            /// </summary>
            /// <param name="obj">Object to compare.</param>
            /// <returns>True if values are equal; otherwise false.</returns>
            public override bool Equals(object? obj)
            {
                // Check if both references point to same object
                if (this == obj)
                    return true;

                // Null check and type safety check
                if (obj == null || this.GetType() != obj.GetType())
                    return false;

                Feet other = (Feet)obj;

                // Use Double.Compare for accurate floating-point comparison
                return this.value.CompareTo(other.value) == 0;
            }

            /// <summary>
            /// Overrides GetHashCode to maintain consistency with Equals.
            /// Required whenever Equals is overridden.
            /// </summary>
            /// <returns>Hash code based on measurement value.</returns>
            public override int GetHashCode()
            {
                return value.GetHashCode();
            }
        }
    }
}